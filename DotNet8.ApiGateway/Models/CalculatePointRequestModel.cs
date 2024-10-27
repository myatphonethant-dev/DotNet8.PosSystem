namespace DotNet8.POS.ApiGateway.Models;

public class CalculatePointRequestModel
{
    public string MemberCode { get; set; }
    public string CouponCode { get; set; }
    public string ReceiptNumber { get; set; }
    public ItemModel[] Items { get; set; }
}