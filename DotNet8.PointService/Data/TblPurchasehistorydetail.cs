using System;
using System.Collections.Generic;

namespace DotNet8.POS.PointService.Data;

public partial class TblPurchasehistorydetail
{
    public string? PurchaseHistoryDetailId { get; set; }

    public string? PurchaseHistoryId { get; set; }

    public string? ItemDescription { get; set; }

    public ulong? AlcoholFree { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? CreatedUserId { get; set; }

    public DateTime? CreatedDateTime { get; set; }
}
