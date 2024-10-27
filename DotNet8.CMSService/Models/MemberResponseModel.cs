namespace DotNet8.POS.CmsService.Models;

public class MemberResponseModel : ApiResponseModel
{
    public List<MemberModel> Data { get; set; }
}