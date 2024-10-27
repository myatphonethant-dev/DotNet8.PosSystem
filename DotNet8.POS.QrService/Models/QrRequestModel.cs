using DotNet8.POS.Shared.Models.Point;

namespace DotNet8.POS.QrService.Models;

public class QrRequestModel
{
    public string QrData { get; set; }
}

public class CouponQrRequestModel
{
    public string QrData { get; set; }

    public string MemberId { get; set; }

    public string MemberCode { get; set; }

    public List<PurchasedItemModel> PurchasedItems { get; set; }
}