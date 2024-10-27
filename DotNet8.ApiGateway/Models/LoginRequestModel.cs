namespace DotNet8.POS.ApiGateway.Models;

public class LoginRequestModel : ApiRequestModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}