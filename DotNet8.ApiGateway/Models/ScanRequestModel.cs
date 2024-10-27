namespace DotNet8.POS.ApiGateway.Models;

public class ScanRequestModel
{
    public string MemberCode { get; set; }
    public string CouponCode { get; set; }
    public ItemModel[] Items { get; set; }
}