using System;
using System.Collections.Generic;

namespace DotNet8.POS.PointService.Data;

public partial class TblPurchasehistory
{
    public string? PurchaseHistoryId { get; set; }

    public string? MemberId { get; set; }

    public int? TotalPoint { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? TranDate { get; set; }

    public string? CreatedUserId { get; set; }

    public DateTime? CreatedDateTime { get; set; }
}
