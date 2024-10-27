using DotNet8.POS.Shared;
using System.Net;
using System.Security.Claims;

namespace DotNet8.POS.ApiGateway.Middleware;

public class JwtTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtTokenService _jwtTokenService;
    private readonly ILogger<JwtTokenMiddleware> _logger;

    public JwtTokenMiddleware(RequestDelegate next, JwtTokenService jwtTokenService, ILogger<JwtTokenMiddleware> logger)
    {
        _next = next;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                var jwtTokenData = _jwtTokenService.ReadJwtToken(token);

                if (DateTime.UtcNow > jwtTokenData.TokenExpired)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Token expired");
                    return;
                }

                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", jwtTokenData.UserId),
                    new Claim("SessionId", jwtTokenData.SessionId),
                });

                context.User = new ClaimsPrincipal(claimsIdentity);
            }
            catch (Exception ex)
            {
                _logger.LogCustomError(ex);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid token");
                return;
            }
        }
        else
        {
            _logger.LogInformation("Unauthorized: No token provided");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorized: No token provided");
            return;
        }

        await _next(context);
    }
}