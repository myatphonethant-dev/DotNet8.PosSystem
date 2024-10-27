namespace DotNet8.POS.PosService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PosController : ControllerBase
{
    private readonly Services.PosService _posService;

    public PosController(Services.PosService posService)
    {
        _posService = posService;
    }

    [HttpPost("scan")]
    public async Task<IActionResult> ScanQr([FromBody] PosRequestModel requestModel)
    {
        var result = await _posService.ScanQr(requestModel);
        return Ok(result);
    }
}