namespace DotNet8.POS.RedisCacheService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RedisCacheController : ControllerBase
{
    private readonly Services.RedisCacheService _cacheService;

    public RedisCacheController(Services.RedisCacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpPost("update-cache")]
    public IActionResult UpdateCache(string couponCode)
    {
        var result = _cacheService.UpdateCache(couponCode);
        return Ok(result);
    }
}