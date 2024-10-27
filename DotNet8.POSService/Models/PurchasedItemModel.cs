namespace DotNet8.POS.PosService.Models;

public class PurchasedItemModel
{
    public string ItemDescription { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}