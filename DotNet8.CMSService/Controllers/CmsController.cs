namespace DotNet8.POS.CmsService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CmsController : ControllerBase
{
    private readonly Services.CmsService _cmsService;

    public CmsController(Services.CmsService cmsService)
    {
        _cmsService = cmsService;
    }

    #region Coupon

    [HttpPost("coupons")]
    public async Task<IActionResult> GetCoupons()
    {
        var coupons = await _cmsService.GetCoupons();

        if (coupons == null || !coupons.Data.Any())
        {
            return NotFound(new { Message = "No coupons found." });
        }

        return Ok(coupons);
    }

    [HttpPost("coupons/create")]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequestModel requestModel)
    {
        var response = await _cmsService.CreateCoupon(requestModel);
        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetCoupons), new { code = requestModel.CouponCode }, response);
        }
        return BadRequest(response);
    }

    [HttpPost("coupons/delete")]
    public async Task<IActionResult> DeleteCoupon([FromBody] DeleteCouponRequestModel requestModel)
    {
        var response = await _cmsService.DeleteCoupon(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpPost("coupons/validate-coupon")]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponRequestModel requestModel)
    {
        var response = await _cmsService.ValidateCoupon(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpPost("coupons/update-count")]
    public async Task<IActionResult> UpdateCouponCount([FromBody] string couponCode)
    {
        var response = await _cmsService.UpdateCouponCount(couponCode);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    #endregion

    #region Member

    [HttpPost("members")]
    public async Task<IActionResult> GetMembers()
    {
        var members = await _cmsService.GetMembers();
        if (members == null || !members.Data.Any())
        {
            return NotFound(new { Message = "No members found." });
        }
        return Ok(members);
    }

    [HttpPost("members/create")]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequestModel requestModel)
    {
        var response = await _cmsService.CreateMember(requestModel);
        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetMembers), new { code = requestModel.MemberCode }, response);
        }
        return BadRequest(response);
    }

    [HttpPost("members/update")]
    public async Task<IActionResult> UpdateMember([FromBody] UpdateMemberRequestModel requestModel)
    {
        var response = await _cmsService.UpdateMember(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpPost("members/delete")]
    public async Task<IActionResult> DeleteMember([FromBody] DeleteMemberRequestModel requestModel)
    {
        var response = await _cmsService.DeleteMember(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpPost("members/validate-member")]
    public async Task<IActionResult> ValidateMember([FromBody] ValidateMemberRequestModel requestModel)
    {
        var response = await _cmsService.ValidateMember(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    #endregion
}