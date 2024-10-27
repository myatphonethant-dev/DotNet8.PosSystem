using DotNet8.POS.CmsService.Models;
using DotNet8.POS.Shared;
using QRCoder;

namespace DotNet8.POS.CmsService.Services;

public class CreateQrService
{
    private readonly ILogger<CreateQrService> _logger;

    public CreateQrService(ILogger<CreateQrService> logger)
    {
        _logger = logger;
    }

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
}