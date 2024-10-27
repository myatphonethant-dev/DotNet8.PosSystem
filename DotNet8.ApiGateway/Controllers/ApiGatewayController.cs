using DotNet8.POS.Shared.Models.Cms;
using DotNet8.POS.Shared.Models.Point;
using DotNet8.POS.Shared.Models.Pos;
using DotNet8.POS.ApiGateway.Services;
using DotNet8.POS.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotNet8.POS.ApiGateway.Controllers;

[Route("api")]
[ApiController]
public class ApiGatewayController : ControllerBase
{
    private readonly HttpClientService _httpClient;

    public ApiGatewayController(HttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    #region CMS

    #region Coupon

    [HttpPost("cms/coupons")]
    public async Task<ActionResult> GetCoupons()
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<CouponResponseModel>("api/cms/coupons", null);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost("cms/coupons/create")]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequestModel requestModel)
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

    [HttpPost("cms/coupons/delete")]
    public async Task<IActionResult> DeleteCoupon([FromBody] DeleteCouponRequestModel requestModel)
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

    [HttpPost("cms/members")]
    public async Task<IActionResult> GetMembers()
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<MemberResponseModel>("api/cms/members", null);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost("cms/members/create")]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberRequestModel requestModel)
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

    [HttpPost("cms/members/update")]
    public async Task<IActionResult> UpdateMember([FromBody] UpdateMemberRequestModel requestModel)
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

    [HttpPost("cms/members/delete")]
    public async Task<IActionResult> DeleteMember([FromBody] DeleteMemberRequestModel requestModel)
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

    #endregion

    #region Point

    [HttpPost("point/calculate")]
    public async Task<IActionResult> CalculatePoints([FromBody] PointCalculationRequestModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid request data" });
        }

        try
        {
            var response = await _httpClient.ExecuteAsync<PointCalculationResponseModel>("api/point/calculate", request);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    #endregion

    #region Pos

    [HttpPost("pos/scanMember")]
    public async Task<IActionResult> ScanMember([FromBody] MemberQrRequestModel request)
    {
        if (string.IsNullOrEmpty(request.QrData))
        {
            return BadRequest(new { Message = "QR data cannot be empty" });
        }

        try
        {
            var response = await _httpClient.ExecuteAsync<MemberResponseModel>("api/pos/scanMember", request);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost("pos/scanCoupon")]
    public async Task<IActionResult> ScanCoupon([FromBody] CouponQrRequestModel request)
    {
        if (string.IsNullOrEmpty(request.QrData))
        {
            return BadRequest(new { Message = "QR data cannot be empty" });
        }

        try
        {
            var response = await _httpClient.ExecuteAsync<CouponResponseModel>("api/pos/scanCoupon", request);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost("pos/checkout")]
    public async Task<IActionResult> Checkout([FromBody] PointCalculationRequestModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid request data" });
        }

        try
        {
            var response = await _httpClient.ExecuteAsync<PointCalculationResponseModel>("api/pos/checkout", request);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    #endregion
}