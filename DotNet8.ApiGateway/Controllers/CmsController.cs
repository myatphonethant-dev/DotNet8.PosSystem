using DotNet8.POS.ApiGateway.Models;
using DotNet8.POS.ApiGateway.Services;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
    {
        try
        {
            var response = await _httpClient.ExecuteAsync<LoginResponseModel>("api/cms/login", request);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
