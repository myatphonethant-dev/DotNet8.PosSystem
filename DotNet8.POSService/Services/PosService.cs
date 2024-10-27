namespace DotNet8.POS.PosService.Services;

public class PosService
{
    private readonly ILogger<PosService> _logger;
    private readonly HttpClientService _httpClient;
    private const string AdfName = "Pos";

    public PosService(ILogger<PosService> logger, HttpClientService httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<PosResponseModel> ScanQr(PosRequestModel requestModel)
    {
        var model = new PosResponseModel();
        var pointRequest = new PointTransferRequestModel();

        try
        {
            #region Member Qr Scan

            if (!string.IsNullOrEmpty(requestModel.MemberQrData))
            {
                var memberValidationResult = await ValidateMemberQr(requestModel.MemberQrData);
                if (!memberValidationResult.isSuccess)
                {
                    model.IsSuccess = false;
                    model.Message = "Invalid Member QR data.";
                    return model;
                }
                model.QrModels.Add(memberValidationResult);

                pointRequest.MemberId = memberValidationResult.Id;
                pointRequest.MemberCode = memberValidationResult.Code;
                pointRequest.PurchasedItems = requestModel.PurchasedItems;
            }
            else
            {
                model.IsSuccess = false;
                model.Message = "Member QR data is required.";
                return model;
            }

            #endregion

            #region Coupon Qr Scan

            if (!string.IsNullOrEmpty(requestModel.CouponQrData))
            {
                var couponValidationResult = await ValidateCouponQr(requestModel.CouponQrData);
                if (!couponValidationResult.isSuccess)
                {
                    model.IsSuccess = false;
                    model.Message = "Invalid Coupon QR data.";
                    return model;
                }
                model.QrModels.Add(couponValidationResult);

                await PassToCache(couponValidationResult.Code);
            }

            #endregion

            #region Pass To Point System

            var earnedPoints = await PassToPoint(pointRequest);

            #endregion

            model.EarnedPoint = earnedPoints.EarnedPoints;
            model.IsSuccess = true;
            model.Message = $"Total {earnedPoints.EarnedPoints} points earned!";
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            model.IsSuccess = false;
            model.Message = ex.Message;
        }

        return model;
    }

    #region Private Method

    private ICollection<Tlv> ConvertQrDataToTlv(string inputBase64)
    {
        var newBytes = Convert.FromBase64String(inputBase64);
        var inputHexstr = BitConverter.ToString(newBytes).Replace("-", string.Empty);
        return Tlv.ParseTlv(inputHexstr);
    }

    private async Task<QrModel> ValidateMemberQr(string inputBase64)
    {
        var model = new QrModel();
        var tlvs = ConvertQrDataToTlv(inputBase64);
        string qrType = string.Empty;

        foreach (var tlv in tlvs)
        {
            if (tlv.HexTag == "85" && Encoding.ASCII.GetString(tlv.Value) != "CPV02")
            {
                model.isSuccess = false;
            }

            if (tlv.HexTag == "61" && Encoding.ASCII.GetString(tlv.Children.First(x => x.HexTag == "4F").Value) != AdfName)
            {
                model.isSuccess = false;
            }

            if (tlv.HexTag == "62")
            {
                qrType = Encoding.ASCII.GetString(tlv.Children.First(x => x.HexTag == "5F21").Value);

                foreach (var childTlv in tlv.Children)
                {
                    if (childTlv.HexTag == "5F22") model.Id = Encoding.ASCII.GetString(childTlv.Value);
                    if (childTlv.HexTag == "5F23") model.Code = Encoding.ASCII.GetString(childTlv.Value);
                }
            }
        }

        var response = await PassToCms(model.Id, model.Code, EnumQrType.Member);

        model.isSuccess = response.isSuccess;
        model.Message = response.Message;
        model.Type = EnumQrType.Member.ToString();
        return model;
    }

    private async Task<QrModel> ValidateCouponQr(string inputBase64)
    {
        var model = new QrModel();
        var tlvs = ConvertQrDataToTlv(inputBase64);
        string qrType = string.Empty;

        foreach (var tlv in tlvs)
        {
            if (tlv.HexTag == "85" && Encoding.ASCII.GetString(tlv.Value) != "CPV02")
            {
                model.isSuccess = false;
            }

            if (tlv.HexTag == "61" && Encoding.ASCII.GetString(tlv.Children.First(x => x.HexTag == "4F").Value) != AdfName)
            {
                model.isSuccess = false;
            }

            if (tlv.HexTag == "62")
            {
                qrType = Encoding.ASCII.GetString(tlv.Children.First(x => x.HexTag == "5F21").Value);

                foreach (var childTlv in tlv.Children)
                {
                    if (childTlv.HexTag == "5F22") model.Id = Encoding.ASCII.GetString(childTlv.Value);
                    if (childTlv.HexTag == "5F23") model.Code = Encoding.ASCII.GetString(childTlv.Value);
                }
            }
        }

        var response = await PassToCms(model.Id, model.Code, EnumQrType.Member);

        model.isSuccess = response.isSuccess;
        model.Message = response.Message;
        model.Type = EnumQrType.Coupon.ToString();
        return model;
    }

    private async Task<QrModel> PassToCms(string id, string code, EnumQrType type)
    {
        var model = new QrModel();
        var endpoint = string.Empty;

        if (type is EnumQrType.Member)
        {
            endpoint = ApiEndpoints.CmsUrl.ValidateMember;
        }
        else if (type is EnumQrType.Coupon)
        {
            endpoint = ApiEndpoints.CmsUrl.ValidateCoupon;
        }

        var request = new QrModel { Id = id, Code = code, };

        var response = await _httpClient.ExecuteAsync<QrModel>(endpoint, HttpMethod.Post, request);

        model.isSuccess = response!.isSuccess;
        model.Message = response.Message;
        return model;
    }

    private async Task<bool> PassToCache(string couponCode)
    {
        var endpoint = ApiEndpoints.CacheUrl.UpdateCache;
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(endpoint, HttpMethod.Post, couponCode);
        if(response!.IsSuccess == false)
        {
            return false;
        }

        return true;
    }

    private async Task<PointTransferResponseModel> PassToPoint(PointTransferRequestModel requestModel)
    {
        var model = new PointTransferResponseModel();
        var endpoint = ApiEndpoints.PointUrl.CalculatePoints;
        var response = await _httpClient.ExecuteAsync<PointTransferResponseModel>(endpoint, HttpMethod.Post, requestModel);
        if (response!.IsSuccess == false)
        {
            return model;
        }

        return response;
    }

    #endregion
}