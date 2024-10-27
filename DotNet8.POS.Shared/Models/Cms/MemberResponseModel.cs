namespace DotNet8.POS.Shared.Models.Cms;

public class MemberResponseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<MemberModel> Data { get; set; }
}