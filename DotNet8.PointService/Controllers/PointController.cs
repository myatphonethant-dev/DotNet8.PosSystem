using DotNet8.POS.PointService.Models;
using DotNet8.POS.PointService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8.PointSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PointController : ControllerBase
{
    private readonly PointService _pointService;

    public PointController(PointService pointService)
    {
        _pointService = pointService;
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> CalculatePoints([FromBody] PointCalculationRequestModel requestModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Invalid request data" });
        }

        var result = await _pointService.CalculatePoints(requestModel);

        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.Message });
        }

        var isUpdated = await _pointService.UpdateMemberPoints(requestModel.MemberCode, result.EarnedPoints);
        if (!isUpdated)
        {
            return StatusCode(500, new { Message = "Failed to update member points" });
        }

        return Ok(new { Message = "Points updated successfully", PointsEarned = result.EarnedPoints });
    }
}