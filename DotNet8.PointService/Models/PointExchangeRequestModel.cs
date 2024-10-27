namespace DotNet8.POS.PointService.Models;

public class PointExchangeRequestModel : ApiRequestModel
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