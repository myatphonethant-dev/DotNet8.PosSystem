namespace DotNet8.POS.PointService.Models;

public class UpdatePointModel : ApiResponseModel
{
    public string MemberId { get; set; }

    public string MemberCode { get; set; }

    public int TotalPoint { get; set; }

    public decimal TotalPrice { get; set; }
}