using DotNet8.POS.ApiGateway.Models;
using DotNet8.POS.ApiGateway.Services;
using DotNet8.POS.CmsService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotNet8.POS.ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CmsController : ControllerBase
{
    private readonly HttpClientService _httpClient;

    public CmsController(HttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    #region Coupon

    [HttpGet("coupons")]
    public async Task<IActionResult> GetCoupons()
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<List<CouponResponseModel>>("api/cms/coupons", null);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost("coupons/create")]
    public async Task<IActionResult> CreateCoupon([FromBody] CouponRequestModel requestModel)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<CouponResponseModel>("api/cms/coupons/create", requestModel);
            if (response.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCoupons), new { code = requestModel.CouponCode }, response);
            }
            return BadRequest(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut("coupons/update")]
    public async Task<IActionResult> UpdateCoupon([FromBody] CouponRequestModel requestModel)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<CouponResponseModel>("api/cms/coupons/update", requestModel);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpDelete("coupons/delete")]
    public async Task<IActionResult> DeleteCoupon([FromBody] CouponRequestModel requestModel)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<CouponResponseModel>("api/cms/coupons/delete", requestModel);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    #endregion

    #region Member

    [HttpGet("members")]
    public async Task<IActionResult> GetMembers()
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<List<MemberResponseModel>>("api/cms/members", null);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost("members/create")]
    public async Task<IActionResult> CreateMember([FromBody] MemberRequestModel requestModel)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<MemberResponseModel>("api/cms/members/create", requestModel);
            if (response.IsSuccess)
            {
                return CreatedAtAction(nameof(GetMembers), new { code = requestModel.MemberCode }, response);
            }
            return BadRequest(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut("members/update")]
    public async Task<IActionResult> UpdateMember([FromBody] MemberRequestModel requestModel)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<MemberResponseModel>("api/cms/members/update", requestModel);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpDelete("members/delete")]
    public async Task<IActionResult> DeleteMember([FromBody] MemberRequestModel requestModel)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<MemberResponseModel>("api/cms/members/delete", requestModel);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    #endregion
}