namespace DotNet8.POS.CmsService.Models;

public class CouponResponseModel : ApiResponseModel
{
    public List<CouponModel> Data { get; set; }
}