namespace DotNet8.POS.PosService.Models;

public class PosResponseModel : ApiResponseModel
{
    public int EarnedPoint { get; set; }

    public List<QrModel> QrModels { get; set; }
}