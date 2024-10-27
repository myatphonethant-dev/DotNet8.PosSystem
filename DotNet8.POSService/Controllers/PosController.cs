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
    public async Task<IActionResult> ScanMember([FromBody] QrRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.QrData))
        {
            return BadRequest(new { Message = "QR data cannot be empty" });
        }

        var memberData = await _qrService.ScanQr(requestModel.QrData);
        if (memberData == null)
        {
            return NotFound(new { Message = "Member not found" });
        }

        return Ok(new { Message = "Member scanned successfully", MemberData = memberData });
    }

    [HttpPost("scanCoupon")]
    public async Task<IActionResult> ScanCoupon([FromBody] QrRequestModel requestModel)
    {
        if (string.IsNullOrEmpty(requestModel.QrData))
        {
            return BadRequest(new { Message = "QR data cannot be empty" });
        }

        var couponData = await _qrService.ScanQr(requestModel.QrData);
        if (couponData == null || couponData.Type != "Coupon")
        {
            return BadRequest(new { Message = "Invalid coupon" });
        }

        var isValid = await _posService.ValidateAndDecrementCouponStock(couponData.Code);
        if (!isValid)
        {
            return BadRequest(new { Message = "Coupon has expired or is no longer available" });
        }

        return Ok(new { Message = "Coupon validated successfully", CouponData = couponData });
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] PointCalculationRequestModel requestModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid request data" });
        }

        var response = await _posService.SendToPointSystem(requestModel);
        if (!response.IsSuccess)
        {
            return BadRequest(new { Message = response.Message });
        }

        return Ok(new { Message = "Points calculated successfully", Details = response });
    }
}
