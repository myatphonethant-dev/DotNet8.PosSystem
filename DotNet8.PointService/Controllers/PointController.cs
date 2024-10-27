namespace DotNet8.POS.PointService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PointController : ControllerBase
{
    private readonly Services.PointService _pointService;

    public PointController(Services.PointService pointService)
    {
        _pointService = pointService;
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> CalculatePoints([FromBody] PointExchangeRequestModel requestModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid request data" });
        }

        var result = await _pointService.CalculatePoints(requestModel);

        if (!result.IsSuccess)
        {
            return BadRequest(new { result.Message });
        }

        return Ok(result);
    }
}