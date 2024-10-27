using DotNet8.POS.ApiGateway.Services;
using DotNet8.POS.CmsService.Models;
using DotNet8.POS.PointService.Models;
using DotNet8.POS.PosService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotNet8.POS.ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PosController : ControllerBase
{
    private readonly HttpClientService _httpClient;

    public PosController(HttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("scanMember")]
    public async Task<IActionResult> ScanMember([FromBody] QrRequestModel request)
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

    [HttpPost("scanCoupon")]
    public async Task<IActionResult> ScanCoupon([FromBody] QrRequestModel request)
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

    [HttpPost("checkout")]
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
}