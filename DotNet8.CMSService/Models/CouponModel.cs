namespace DotNet8.POS.CmsService.Models;

public class CouponModel
{
    public string CouponCode { get; set; }

    public string CouponName { get; set; }

    public decimal? DiscountAmount { get; set; }

    public int? AvailableQuantity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string CouponQrFilePath { get; set; }
}