namespace DotNet8.POS.Shared.Models;

public class ApiResponseModel
{
    public object Data { get; set; }

    public bool IsSuccess { get; set; }

    public string Message { get; set; }
}