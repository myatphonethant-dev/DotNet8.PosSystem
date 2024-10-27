namespace DotNet8.POS.Shared.Models.Login;

public class LoginResponseModel
{
    public string UserId { get; set; }
    public string SessionId { get; set; }
    public string AccessToken { get; set; }
}