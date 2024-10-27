using DotNet8.POS.ApiGateway.Models;
using DotNet8.POS.ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotNet8.POS.ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PointController : ControllerBase
{
    private readonly HttpClientService _httpClient;

    public PointController(HttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> CalculatePoints([FromBody] CalculatePointRequestModel request)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<CalculatePointResponseModel>("api/point/calculate", request);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}

public class CalculatePointResponseModel
{

}