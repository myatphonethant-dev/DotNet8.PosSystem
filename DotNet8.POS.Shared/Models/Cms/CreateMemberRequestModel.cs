using DotNet8.POS.Shared.Models;

namespace DotNet8.POS.Shared.Models.Cms;

public class CreateMemberRequestModel : ApiRequestModel
{
    public string MemberCode { get; set; }

    public string Name { get; set; }

    public string PhoneNo { get; set; }
}

public class MemberModel
{
    public string MemberCode { get; set; }

    public string Name { get; set; }

    public string PhoneNo { get; set; }

    public int? TotalPoints { get; set; }

    public decimal? TotalPurchasedAmount { get; set; }

    public string MemberQrFilePath { get; set; }

    public string CreatedUserId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string ModifiedUserId { get; set; }

    public DateTime ModifiedDateTime { get; set; }

    public ulong DelFlag { get; set; }
}

public class DeleteMemberRequestModel : ApiRequestModel
{
    public string MemberCode { get; set; }
}

public class UpdateMemberRequestModel : ApiRequestModel
{
    public string MemberCode { get; set; }

    public string PhoneNo { get; set; }
}