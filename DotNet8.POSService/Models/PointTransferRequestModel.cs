namespace DotNet8.POS.PosService.Models;

public class PointTransferRequestModel : ApiRequestModel
{
    public string MemberId { get; set; }

    public string MemberCode { get; set; }

    public List<PurchasedItemModel> PurchasedItems { get; set; }
}