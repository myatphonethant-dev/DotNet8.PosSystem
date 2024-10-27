namespace DotNet8.POS.Shared.Models.Cms;

public class CouponResponseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public List<CouponModel> Data { get; set; }
}