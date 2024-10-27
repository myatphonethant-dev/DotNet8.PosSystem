﻿using DotNet8.POS.DbService.PosDbContext;
using DotNet8.POS.PosService.Models;
using DotNet8.POS.Shared;
using DotNet8.POS.Shared.Models;

namespace DotNet8.POS.PosService.Services;

public class QrService
{
    private readonly ILogger<QrService> _logger;
    private readonly PosDbContext _context;

    public QrService(ILogger<QrService> logger, PosDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    private const string AdfName = "Pos";

    public async Task<QrResponseModel> ScanQr(string qrData)
    {
        var model = new QrResponseModel();
        try
        {
            var tlvs = ConvertQrDataToTlv(qrData);
            string qrType = string.Empty;

            foreach (var tlv in tlvs)
            {
                if (tlv.HexTag == "85")
                {
                    string cpv = Encoding.ASCII.GetString(tlv.Value);
                    if (cpv != "CPV02")
                    {
                        model.Message = "Failed";
                        goto result;
                    }
                }

                if (tlv.HexTag == "61")
                {
                    var childTlv61 = tlv.Children.Where(x => x.HexTag == "4F").
                         FirstOrDefault();
                    string adfName = Encoding.ASCII.GetString(childTlv61!.Value);
                    if (adfName != AdfName)
                    {
                        model.Message = "Failed";
                        goto result;
                    }
                }

                if (tlv.HexTag == "62")
                {
                    var childTlv62 = tlv.Children.Where(x => x.HexTag == "5F21").
                         FirstOrDefault();
                    qrType = Encoding.ASCII.GetString(childTlv62!.Value);
                }

                if (qrType != "Member" || qrType != "Coupon")
                {
                    model.Message = "Failed";
                    goto result;
                }
                else if (qrType is "Member")
                {
                    model = await ValidateMemberQr(qrData);
                    model.Type = qrType;
                }
                else if (qrType is "Coupon")
                {
                    model = await ValidateCouponQr(qrData);
                    model.Type = qrType;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    result:
        return model;
    }

    private ICollection<Tlv> ConvertQrDataToTlv(string inputBase64)
    {
        byte[] newBytes = Convert.FromBase64String(inputBase64);
        string inputHexstr = BitConverter.ToString(newBytes).Replace("-", string.Empty);
        ICollection<Tlv> tlvs = Tlv.ParseTlv(inputHexstr);
        return tlvs;
    }

    private async Task<QrResponseModel> ValidateMemberQr(string inputBase64)
    {
        var model = new QrResponseModel();
        string merchantUserId = string.Empty;
        var tlvs = ConvertQrDataToTlv(inputBase64);
        foreach (var tlv in tlvs)
        {
            if (tlv.HexTag == "62")
            {
                foreach (var childTlv in tlv.Children)
                {
                    if (childTlv.HexTag == "5F22") model.Id = Encoding.ASCII.GetString(childTlv.Value);
                    if (childTlv.HexTag == "5F23") model.Code = Encoding.ASCII.GetString(childTlv.Value);
                }
            }
        }

        var member = await _context.TblMembers
                    .AsNoTracking().
                    FirstOrDefaultAsync(x =>
                    x.MemberId == model.Id &&
                    x.MemberCode == model.Code);
        if (member is null)
        {
            model.Message = "Failed";
            goto result;
        }
        model.Message = "Success";
    result:
        return model;
    }

    private async Task<QrResponseModel> ValidateCouponQr(string inputBase64)
    {
        var model = new QrResponseModel();
        string merchantUserId = string.Empty;
        var tlvs = ConvertQrDataToTlv(inputBase64);
        foreach (var tlv in tlvs)
        {
            if (tlv.HexTag == "62")
            {
                foreach (var childTlv in tlv.Children)
                {
                    if (childTlv.HexTag == "5F22") model.Id = Encoding.ASCII.GetString(childTlv.Value);
                    if (childTlv.HexTag == "5F23") model.Code = Encoding.ASCII.GetString(childTlv.Value);
                }
            }
        }

        var member = await _context.TblCoupons
                    .AsNoTracking().
                    FirstOrDefaultAsync(x =>
                    x.CouponId == model.Id &&
                    x.CouponCode == model.Code);
        if (member is null)
        {
            model.Message = "Failed";
            goto result;
        }
        model.Message = "Success";
    result:
        return model;
    }
}