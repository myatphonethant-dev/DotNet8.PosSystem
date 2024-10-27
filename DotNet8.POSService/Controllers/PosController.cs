using DotNet8.POS.CmsService.Models;
using DotNet8.POS.PointService.Models;
using DotNet8.POS.PosService.Models;
using DotNet8.POS.PosService.Services;

namespace DotNet8.POS.PosService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PosController : ControllerBase
{
    private readonly Services.PosService _posService;
    private readonly QrService _qrService;

    public PosController(Services.PosService posService, QrService qrService)
    {
        _posService = posService;
        _qrService = qrService;
    }

    [HttpPost("scanMember")]
    public async Task<IActionResult> ScanMember([FromBody] MemberQrRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.QrData))
        {
            return BadRequest(new { Message = "QR data cannot be empty" });
        }

        var memberData = await _qrService.ScanQr(requestModel.QrData);
        if (memberData == null || memberData.isSuccess == false)
        {
            return BadRequest(new { Message = memberData!.Message });
        }

        return Ok(new { MemberData = memberData });
    }

    [HttpPost("scanCoupon")]
    public async Task<IActionResult> ScanCoupon([FromBody] CouponQrRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.QrData))
        {
            return BadRequest(new { Message = "QR data cannot be empty" });
        }

        var couponData = await _qrService.ScanQr(requestModel.QrData);
        if (couponData == null || couponData.isSuccess == false)
        {
            return BadRequest(new { Message = couponData!.Message });
        }

        if (couponData.Type == EnumQrType.Coupon.ToString())
        {
            var isValid = await _posService.ValidateAndDecrementCouponStock(couponData.Code);
            if (isValid is true)
            {
                return BadRequest(new { Message = "Coupon has expired or is no longer available" });
            }

            var checkOutRequest = new PointCalculationRequestModel()
            {
                MemberId = requestModel.MemberId,
                MemberCode = requestModel.MemberCode,
                PurchasedItems = requestModel.PurchasedItems
            };
            var response = await _posService.SendToPointSystem(checkOutRequest);

            if (!response.IsSuccess)
            {
                return BadRequest(new { Message = response.Message });
            }

            return Ok(new { Message = "Points calculated successfully", Details = response });
        }
        return Ok(new { Message = "Coupon validated successfully" });
    }
}