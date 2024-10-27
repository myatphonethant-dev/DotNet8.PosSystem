namespace DotNet8.POS.Shared.Models.Point;

public class PointCalculationResponseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int EarnedPoints { get; set; }
}