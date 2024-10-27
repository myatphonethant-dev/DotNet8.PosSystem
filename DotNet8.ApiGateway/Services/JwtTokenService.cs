using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DotNet8.POS.ApiGateway.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(TblLogin login)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = _configuration["Jwt:Key"];
        var key = Encoding.UTF8.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Jti, login.SessionId),
                new Claim("UserId", login.UserId),
                new Claim("SessionId", login.SessionId),
                new Claim("TokenExpired", DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:TokenExpirationInMinutes"])).ToString("o"))
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:TokenExpirationInMinutes"])),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        var expiredClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "TokenExpired")
                           ?? throw new Exception("TokenExpired is required.");
        var tokenExpired = Convert.ToDateTime(expiredClaim?.Value);

        var userIdClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "UserId")
                          ?? throw new Exception("UserId is required.");
        var sessionIdClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "SessionId")
                             ?? throw new Exception("SessionId is required.");

        var model = new TblLogin
        {
            UserId = userIdClaim.Value,
            SessionId = sessionIdClaim.Value
        };

        var refreshToken = DateTime.Now > tokenExpired ? GenerateAccessToken(model) : token;
        return refreshToken;
    }

    public ReadJwtToken ReadJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        var expiredClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "TokenExpired")
                           ?? throw new Exception("TokenExpired is required.");
        var tokenExpired = Convert.ToDateTime(expiredClaim?.Value);

        var userIdClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "UserId")
                          ?? throw new Exception("UserId is required.");
        var sessionIdClaim = decodedToken.Claims.FirstOrDefault(x => x.Type == "SessionId")
                             ?? throw new Exception("SessionId is required.");

        var model = new ReadJwtToken
        {
            UserId = userIdClaim.Value,
            SessionId = sessionIdClaim.Value,
            TokenExpired = tokenExpired
        };

        return model;
    }
}

public class ReadJwtToken
{
    public string UserId { get; set; }
    public string SessionId { get; set; }
    public DateTime TokenExpired { get; set; }
}

public class TblLogin
{
    public string UserId { get; set; }
    public string SessionId { get; set; }
}