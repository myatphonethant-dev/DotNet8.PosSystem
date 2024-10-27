using System;
using System.Collections.Generic;

namespace DotNet8.POS.DbService.PosDbContext;

public partial class TblCoupon
{
    public string CouponId { get; set; } = null!;

    public string CouponCode { get; set; } = null!;

    public string CouponName { get; set; } = null!;

    public decimal DiscountAmount { get; set; }

    public int AvailableQuantity { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string CouponQrFilePath { get; set; } = null!;

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public ulong DelFlag { get; set; }
}
