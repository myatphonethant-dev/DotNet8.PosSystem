using DotNet8.POS.Shared.Models;

namespace DotNet8.POS.Shared.Models.Point;

public class PointCalculationRequestModel : ApiRequestModel
{
    public string MemberId { get; set; }

    public string MemberCode { get; set; }

    public List<PurchasedItemModel> PurchasedItems { get; set; }
}

public class PurchasedItemModel
{
    public string ItemDescription { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}

public class PurchaseHistoryModel
{
    public string PurchaseHistoryId { get; set; }
    public string MemberId { get; set; }
    public int TotalPoint { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime TranDate { get; set; }
    public string CreatedUserId { get; set; }
    public DateTime CreatedDateTime { get; set; }
}

public class PurchaseHistoryDetailModel
{
    public string PurchaseHistoryDetailId { get; set; }
    public string PurchaseHistoryId { get; set; }
    public string ItemDescription { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public string CreatedUserId { get; set; }
    public DateTime CreatedDateTime { get; set; }
}