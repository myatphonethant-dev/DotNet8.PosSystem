namespace DotNet8.POS.CmsService.Models;

public class CreateCouponRequestModel : ApiRequestModel
{
    public string CouponCode { get; set; }

    public string CouponName { get; set; }

    public decimal DiscountAmount { get; set; }

    public int AvailableQuantity { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}

public class DeleteCouponRequestModel : ApiRequestModel
{
    public string CouponCode { get; set; }
}

public class ValidateCouponRequestModel : ApiRequestModel
{
    public string CouponId { get; set; }
    
    public string CouponCode { get; set; }
}