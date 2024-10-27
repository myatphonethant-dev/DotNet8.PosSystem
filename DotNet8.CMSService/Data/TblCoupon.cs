using System;
using System.Collections.Generic;

namespace DotNet8.POS.CmsService.Data;

public partial class TblCoupon
{
    public string? CouponId { get; set; }

    public string? CouponCode { get; set; }

    public string? CouponName { get; set; }

    public decimal? DiscountAmount { get; set; }

    public int? AvailableQuantity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? CouponQrFilePath { get; set; }

    public string? CreatedUserId { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public ulong? DelFlag { get; set; }
}
