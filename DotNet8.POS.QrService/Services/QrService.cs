using DotNet8.POS.QrService.Models;
using System.Text;

namespace DotNet8.POS.QrService.Services;

public class QrService
{
    private readonly ILogger<QrService> _logger;
    private const string AdfName = "Pos";

    public QrService(ILogger<QrService> logger)
    {
        _logger = logger;
    }

    public async Task<QrResponseModel> ScanQr(string qrData)
    {
        var model = new QrResponseModel();
        try
        {
            var tlvs = ConvertQrDataToTlv(qrData);
            string qrType = string.Empty;

            foreach (var tlv in tlvs)
            {
                if (tlv.HexTag == "85" && Encoding.ASCII.GetString(tlv.Value) != "CPV02")
                {
                    model.isSuccess = false;
                    goto result;
                }

                if (tlv.HexTag == "61" && Encoding.ASCII.GetString(tlv.Children.First(x => x.HexTag == "4F").Value) != AdfName)
                {
                    model.isSuccess = false;
                    goto result;
                }

                if (tlv.HexTag == "62")
                {
                    qrType = Encoding.ASCII.GetString(tlv.Children.First(x => x.HexTag == "5F21").Value);
                }

                if (qrType == "Member")
                {
                    model = await ValidateMemberQr(qrData);
                    model.Type = qrType;
                    goto result;
                }
                else if (qrType == "Coupon")
                {
                    model = await ValidateCouponQr(qrData);
                    model.Type = qrType;
                    goto result;
                }
                else
                {
                    model.isSuccess = false;
                    goto result;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning QR code.");
        }
    result:
        return model;
    }

    private ICollection<Tlv> ConvertQrDataToTlv(string inputBase64)
    {
        var newBytes = Convert.FromBase64String(inputBase64);
        var inputHexstr = BitConverter.ToString(newBytes).Replace("-", string.Empty);
        return Tlv.ParseTlv(inputHexstr);
    }

    private async Task<QrResponseModel> ValidateMemberQr(string inputBase64)
    {
        var model = new QrResponseModel();
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
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.MemberId == model.Id && x.MemberCode == model.Code);
        if (member == null)
        {
            model.isSuccess = false;
            model.Message = "Member Not Found";
            return model;
        }

        model.isSuccess = true;
        return model;
    }

    private async Task<QrResponseModel> ValidateCouponQr(string inputBase64)
    {
        var model = new QrResponseModel();
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

        var coupon = await _context.TblCoupons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CouponId == model.Id && x.CouponCode == model.Code);
        if (coupon == null)
        {
            model.isSuccess = false;
            model.Message = "Coupon Not Found";
            return model;
        }

        model.isSuccess = true;
        return model;
    }
}