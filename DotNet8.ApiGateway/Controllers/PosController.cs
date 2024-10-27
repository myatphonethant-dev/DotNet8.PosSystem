using DotNet8.POS.ApiGateway.Models;
using DotNet8.POS.ApiGateway.Services;
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

    [HttpPost("scan")]
    public async Task<IActionResult> Scan([FromBody] ScanRequestModel request)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<ScanResponseModel>("api/pos/scan", request);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}

public class ScanResponseModel
{

}