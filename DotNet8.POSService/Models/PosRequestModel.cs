namespace DotNet8.POS.PosService.Models;

public class PosRequestModel : ApiRequestModel
{
    public string MemberQrData { get; set; }

    public string CouponQrData { get; set; }

    public List<PurchasedItemModel> PurchasedItems { get; set; }
}