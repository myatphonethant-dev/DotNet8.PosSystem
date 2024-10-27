namespace DotNet8.POS.CmsService.Services;

public class CmsService
{
    private readonly ILogger<CmsService> _logger;
    private readonly CmsDbContext _context;

    public CmsService(ILogger<CmsService> logger, CmsDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    #region Coupon

    public async Task<CouponResponseModel> GetCoupons()
    {
        var model = new CouponResponseModel();
        var data = await _context.TblCoupons
                   .AsNoTracking()
                   .Where(x => x.DelFlag == 0)
                   .ToListAsync();

        model.Data = data.Select(c => new CouponModel
        {
            CouponCode = c.CouponCode,
            CouponName = c.CouponName,
            DiscountAmount = c.DiscountAmount,
            AvailableQuantity = c.AvailableQuantity,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            CouponQrFilePath = c.CouponQrFilePath
        }).ToList();
        model.IsSuccess = true;
        model.Message = "Successfully Retrieved Data.";
        return model;
    }

    public async Task<CouponResponseModel> CreateCoupon(CreateCouponRequestModel requestModel)
    {
        var model = new CouponResponseModel();
        try
        {
            var item = await _context.TblCoupons
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.CouponCode == requestModel.CouponCode &&
                    c.DelFlag == 0);

            if (item is not null)
            {
                model.IsSuccess = false;
                model.Message = "Coupon already exist.";
                return model;
            }

            var coupon = new TblCoupon()
            {
                CouponId = Guid.NewGuid().ToString(),
                CouponCode = requestModel.CouponCode,
                CouponName = requestModel.CouponName,
                DiscountAmount = requestModel.DiscountAmount,
                AvailableQuantity = requestModel.AvailableQuantity,
                StartDate = requestModel.StartDate,
                EndDate = requestModel.EndDate,
                CouponQrFilePath = await GenerateQrCode(requestModel.CouponCode, requestModel.CouponName, EnumQrType.Coupon),
                CreatedUserId = requestModel.UserId,
                CreatedDateTime = DateTime.UtcNow,
                DelFlag = 0
            };

            await _context.TblCoupons.AddAsync(coupon);
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Coupon Successfully Saved.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<CouponResponseModel> DeleteCoupon(DeleteCouponRequestModel requestModel)
    {
        var model = new CouponResponseModel();
        try
        {
            var coupon = await _context.TblCoupons
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.CouponCode == requestModel.CouponCode &&
                    c.DelFlag == 0);

            if (coupon is null)
            {
                model.IsSuccess = false;
                model.Message = "Coupon not found.";
                return model;
            }

            coupon.DelFlag = 1;
            coupon.ModifiedUserId = requestModel.UserId;
            coupon.ModifiedDateTime = DateTime.UtcNow;

            _context.Entry(coupon).State = EntityState.Modified;
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Coupon successfully deleted.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<CouponResponseModel> ValidateCoupon(ValidateCouponRequestModel requestModel)
    {
        var model = new CouponResponseModel();
        try
        {
            var coupon = await _context.TblCoupons
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.DelFlag == 0 &&
                x.CouponId == requestModel.CouponId && 
                x.CouponCode == requestModel.CouponCode);

            if (coupon == null)
            {
                model.IsSuccess = false;
                model.Message = "Coupon Not Found";
            }

            model.IsSuccess = true;
            model.Message = "Success";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<ApiResponseModel> UpdateCouponCount(string couponCode)
    {
        var model = new ApiResponseModel();
        var coupon = await _context.TblCoupons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.CouponCode == couponCode &&
                        c.AvailableQuantity > 0);
        if (coupon == null)
        {
            model.IsSuccess = false;
            model.Message = "Coupon not found.";
            return model;
        }
        coupon.AvailableQuantity--;

        _context.Entry(coupon).State = EntityState.Modified;
        await _context.SaveAndDetachAsync();

        model.IsSuccess = true;
        model.Message = "Success";
        return model;
    }

    #endregion

    #region Member

    public async Task<MemberResponseModel> GetMembers()
    {
        var model = new MemberResponseModel();
        var data = await _context.TblMembers
                    .AsNoTracking()
                    .Where(x => x.DelFlag == 0)
                    .ToListAsync();

        model.Data = data.Select(c => new MemberModel
        {
            MemberCode = c.MemberCode,
            Name = c.Name,
            PhoneNo = c.PhoneNo,
            TotalPoints = c.TotalPoints,
            TotalPurchasedAmount = c.TotalPurchasedAmount,
            MemberQrFilePath = c.MemberQrFilePath,
            CreatedUserId = c.CreatedUserId,
            CreatedDateTime = c.CreatedDateTime,
        }).ToList();
        model.IsSuccess = true;
        model.Message = "Successfully Retrieved Data.";
        return model;
    }

    public async Task<MemberResponseModel> CreateMember(CreateMemberRequestModel requestModel)
    {
        var model = new MemberResponseModel();
        try
        {
            var member = new TblMember()
            {
                MemberId = Guid.NewGuid().ToString(),
                MemberCode = requestModel.MemberCode,
                Name = requestModel.Name,
                PhoneNo = requestModel.PhoneNo,
                TotalPoints = 0,
                TotalPurchasedAmount = 0,
                MemberQrFilePath = await GenerateQrCode(requestModel.MemberCode, requestModel.Name, EnumQrType.Member),
                CreatedUserId = requestModel.UserId,
                CreatedDateTime = DateTime.UtcNow,
                DelFlag = 0
            };

            await _context.TblMembers.AddAsync(member);
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Coupon Successfully Saved.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<MemberResponseModel> UpdateMember(UpdateMemberRequestModel requestModel)
    {
        var model = new MemberResponseModel();
        try
        {
            var member = await _context.TblMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(m =>
                    m.MemberCode == requestModel.MemberCode &&
                    m.DelFlag == 0);

            if (member is null)
            {
                model.IsSuccess = false;
                model.Message = "Member not found.";
                return model;
            }

            member.PhoneNo = requestModel.PhoneNo;
            member.ModifiedUserId = requestModel.UserId;
            member.ModifiedDateTime = DateTime.UtcNow;

            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Member successfully updated.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<MemberResponseModel> DeleteMember(DeleteMemberRequestModel requestModel)
    {
        var model = new MemberResponseModel();
        try
        {
            var member = await _context.TblMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(m =>
                    m.MemberCode == requestModel.MemberCode &&
                    m.DelFlag == 0);

            if (member is null)
            {
                model.IsSuccess = false;
                model.Message = "Member not found.";
                return model;
            }

            member.DelFlag = 1;
            member.ModifiedUserId = requestModel.UserId;
            member.ModifiedDateTime = DateTime.UtcNow;

            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Member successfully deleted.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<MemberResponseModel> ValidateMember(ValidateMemberRequestModel requestModel)
    {
        var model = new MemberResponseModel();
        try
        {
            var member = await _context.TblMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.DelFlag == 0 && 
                x.MemberId == requestModel.MemberId && 
                x.MemberCode == requestModel.MemberCode);

            if (member is null)
            {
                model.IsSuccess = false;
                model.Message = "Member Not Found";
            }

            model.IsSuccess = true;
            model.Message = "Success";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    #endregion

    #region Create Qr

    public async Task<string> GenerateQrCode(string couponCode, string couponName, EnumQrType type)
    {
        var imageUrl = string.Empty;
        string encryptedData = EncryptData(couponCode, couponName, type);

        QRCodeGenerator qrGenerator = new QRCodeGenerator();

        QRCodeData qrData = qrGenerator.CreateQrCode(encryptedData, QRCodeGenerator.ECCLevel.Q);

        BitmapByteQRCode bitmapQrCode = new BitmapByteQRCode(qrData);

        byte[] qrByte = bitmapQrCode.GetGraphic(10);
        string str64Base = Convert.ToBase64String(qrByte);

        if (type == EnumQrType.Member)
        {
            imageUrl = WriteQrFile(str64Base, type.ToString());
        }
        else if (type == EnumQrType.Coupon)
        {
            imageUrl = WriteQrFile(str64Base, type.ToString());
        }

        return imageUrl;
    }

    private string EncryptData(string code, string name, EnumQrType type)
    {
        string adfName = "Pos";

        #region CPV02

        var hexString = "85";
        var root = "CPV02";
        var hexQrType = string.Empty;
        hexString += StringToHex(root);

        #endregion

        #region ADF 61

        var adFstring = "61";
        var hexAdfName = "4F" + StringToHex(adfName);
        var totalAdfStringCount = hexAdfName.Length / 2;
        var hexTotalAdfStringCount = String.Format("{0:X2}", totalAdfStringCount);

        hexString += adFstring + hexTotalAdfStringCount + hexAdfName;

        #endregion

        #region Account Info 62

        var accInfo = "62";

        if (type == EnumQrType.Member)
        {
            hexQrType = "5F21" + StringToHex("Member");
        }
        else if (type == EnumQrType.Coupon)
        {
            hexQrType = "5F21" + StringToHex("Coupon");
        }

        var hexCode = "5F22" + StringToHex(code);
        var hexName = "5F23" + StringToHex(name);

        var totalInfoStringCount = (hexQrType.Length + hexCode.Length +
                                    hexName.Length) / 2;

        var hexTotalInfoStringCount = String.Format("{0:X2}", totalInfoStringCount);

        #endregion

        hexString += accInfo + hexTotalInfoStringCount + hexQrType + hexCode + hexName;

        byte[] data = ConvertFromStringToHex(hexString);
        string base64 = Convert.ToBase64String(data);
        return base64;
    }

    private string StringToHex(string stringVal)
    {
        var output = string.Empty;
        var stringCount = stringVal.Length;
        output += String.Format("{0:X2}", stringCount);
        char[] values = stringVal.ToCharArray();
        foreach (char letter in values)
        {
            int value = Convert.ToInt32(letter);
            string hexOutput = String.Format("{0:X}", value);
            output += hexOutput;
        }

        return output;
    }

    private byte[] ConvertFromStringToHex(string inputHex)
    {
        inputHex = inputHex.Replace(" ", "");

        byte[] resultantArray = new byte[inputHex.Length / 2];
        for (int i = 0; i < resultantArray.Length; i++)
        {
            try
            {
                var substring = inputHex.Substring(i * 2, 2);
                resultantArray[i] = Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            catch (Exception ex)
            {
                _logger.LogCustomError(ex);
            }
        }

        return resultantArray;
    }

    public string WriteQrFile(string imageBase64, string folderName)
    {
        var imageUrl = string.Empty;
        try
        {
            string strImageBase64 = imageBase64.UrlDecode().Replace(@"\n", "").Replace(" ", "+");

            string imageGuid = Guid.NewGuid().ToString();
            string sFileExtension = ".jpg";
            string fileName = imageGuid + sFileExtension;

            string projectDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(projectDirectory, folderName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fullFilePath = Path.Combine(filePath, fileName);

            File.WriteAllBytes(fullFilePath, Convert.FromBase64String(strImageBase64));

            imageUrl = Path.Combine(folderName, fileName).Replace("\\", "/");

        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
        return imageUrl;
    }

    #endregion
}