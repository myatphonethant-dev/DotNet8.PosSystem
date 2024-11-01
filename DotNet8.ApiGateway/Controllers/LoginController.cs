﻿namespace DotNet8.POS.ApiGateway.Controllers;

[ApiExplorerSettings(GroupName = "Login")]
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    public LoginController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromBody] JsonRequestModel requestModel)
    {
        var user = requestModel.RequestJsonString.ToObject<LoginResponseModel>();
        if (user is null) return Ok();

        var accessToken = _jwtTokenService.ReadJwtToken(user.AccessToken);
        var claims = new List<Claim>
        {
            new Claim("UserId",user.UserId),
            new Claim("SessionId",user.SessionId),
            new Claim("AccessToken",user.AccessToken)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);
        return Ok();
    }
}