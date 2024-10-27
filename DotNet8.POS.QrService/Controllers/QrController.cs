using Microsoft.AspNetCore.Mvc;

namespace DotNet8.POS.QrService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QrController : ControllerBase
{
    private readonly Services.QrService _qrService;

    public QrController(Services.QrService qrService)
    {
        _qrService = qrService;
    }

    [HttpPost("scan")]
    public async Task<IActionResult> ScanQr([FromBody] string qrData)
    {
        var result = await _qrService.ScanQr(qrData);
        return Ok(result);
    }
}