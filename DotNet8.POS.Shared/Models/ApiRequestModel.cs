namespace DotNet8.POS.Shared.Models;

public class ApiRequestModel
{
    public string UserId { get; set; }

    public string SessionId { get; set; }

    public string AccessToken { get; set; }

    public object Data { get; set; }
}