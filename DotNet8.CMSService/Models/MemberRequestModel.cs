namespace DotNet8.POS.CmsService.Models;

public class CreateMemberRequestModel : ApiRequestModel
{
    public string MemberCode { get; set; }

    public string Name { get; set; }

    public string PhoneNo { get; set; }
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

public class ValidateMemberRequestModel : ApiRequestModel
{
    public string MemberId { get; set; }

    public string MemberCode { get; set; }
}