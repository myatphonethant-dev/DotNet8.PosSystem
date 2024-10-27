using DotNet8.POS.CmsService.Models;
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

    [HttpGet("coupons")]
    public async Task<IActionResult> GetCoupons()
    {
        var coupons = await _couponService.GetCoupons();
        return Ok(coupons);
    }

    [HttpPost("coupons/create")]
    public async Task<IActionResult> CreateCoupon([FromBody] CouponRequestModel requestModel)
    {
        var response = await _couponService.CreateCoupon(requestModel);
        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetCoupons), new { code = requestModel.CouponCode }, response);
        }
        return BadRequest(response);
    }

    [HttpPut("coupons/update")]
    public async Task<IActionResult> UpdateCoupon([FromBody] CouponRequestModel requestModel)
    {
        var response = await _couponService.UpdateCoupon(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpDelete("coupons/delete")]
    public async Task<IActionResult> DeleteCoupon([FromBody] CouponRequestModel requestModel)
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

    [HttpGet("members")]
    public async Task<IActionResult> GetMembers()
    {
        var members = await _memberService.GetMembers();
        return Ok(members);
    }

    [HttpPost("members/create")]
    public async Task<IActionResult> CreateMember([FromBody] MemberRequestModel requestModel)
    {
        var response = await _memberService.CreateCoupon(requestModel);
        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetMembers), new { code = requestModel.MemberCode }, response);
        }
        return BadRequest(response);
    }

    [HttpPut("members/update")]
    public async Task<IActionResult> UpdateMember([FromBody] MemberRequestModel requestModel)
    {
        var response = await _memberService.UpdateMember(requestModel);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpDelete("members/delete")]
    public async Task<IActionResult> DeleteMember([FromBody] MemberRequestModel requestModel)
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