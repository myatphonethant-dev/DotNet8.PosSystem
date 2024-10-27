using DotNet8.POS.Shared.Models;

namespace DotNet8.POS.CmsService.Models;

public class MemberRequestModel : ApiRequestModel
{
    public string MemberCode { get; set; }

    public string Name { get; set; }

    public string PhoneNo { get; set; }
}