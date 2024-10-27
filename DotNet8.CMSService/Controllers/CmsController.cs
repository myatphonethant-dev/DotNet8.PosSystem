using DotNet8.POS.Shared.Models.Cms;
using DotNet8.POS.CmsService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8.POS.CmsService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CmsController : ControllerBase
{
    private readonly CouponService _couponService;
    private readonly MemberService _memberService;

    public CmsController(CouponService couponService, MemberService memberService)
    {
        _couponService = couponService;
        _memberService = memberService;
    }

    #region Coupon

    [HttpPost("coupons")]
    public async Task<IActionResult> GetCoupons()
    {
        var coupons = await _couponService.GetCoupons();

        if (coupons == null || !coupons.Data.Any())
        {
            return NotFound(new { Message = "No coupons found." });
        }

        return Ok(coupons);
    }

    [HttpPost("coupons/create")]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequestModel requestModel)
    {
        var response = await _couponService.CreateCoupon(requestModel);
        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetCoupons), new { code = requestModel.CouponCode }, response);
        }
        return BadRequest(response);
    }

    [HttpPost("coupons/delete")]
    public async Task<IActionResult> DeleteCoupon([FromBody] DeleteCouponRequestModel requestModel)
    {
        var response = await _couponService.DeleteCoupon(requestModel);
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
        var members = await _memberService.GetMembers();
        if (members == null || !members.Data.Any())
        {
            return NotFound(new { Message = "No members found." });
        }
        return Ok(members);
    }

    [HttpPost("members/create")]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequestModel requestModel)
    {
        var response = await _memberService.CreateMember(requestModel);
        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetMembers), new { code = requestModel.MemberCode }, response);
        }
        return BadRequest(response);
    }

    [HttpPost("members/update")]
    public async Task<IActionResult> UpdateMember([FromBody] UpdateMemberRequestModel requestModel)
    {
        var response = await _memberService.UpdateMember(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpPost("members/delete")]
    public async Task<IActionResult> DeleteMember([FromBody] DeleteMemberRequestModel requestModel)
    {
        var response = await _memberService.DeleteMember(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    #endregion
}