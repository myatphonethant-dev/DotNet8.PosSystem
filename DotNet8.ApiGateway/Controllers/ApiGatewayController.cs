namespace DotNet8.POS.ApiGateway.Controllers;

[Route("api")]
[ApiController]
public class ApiGatewayController : ControllerBase
{
    private readonly HttpClientService _httpClient;

    public ApiGatewayController(HttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    #region CMS

    #region Coupon

    [HttpPost("cms/coupons")]
    public async Task<ActionResult> GetCoupons()
    {
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(ApiEndpoints.CmsUrl.GetCoupons, HttpMethod.Post, null);
        if (response!.IsSuccess) { return Ok(response); }
        return BadRequest(response);
    }

    [HttpPost("cms/coupons/create")]
    public async Task<IActionResult> CreateCoupon([FromBody] ApiRequestModel requestModel)
    {
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(ApiEndpoints.CmsUrl.CreateCoupon, HttpMethod.Post, requestModel);
        if (response!.IsSuccess) { return Ok(response); }
        return BadRequest(response);
    }

    [HttpPost("cms/coupons/delete")]
    public async Task<IActionResult> DeleteCoupon([FromBody] ApiRequestModel requestModel)
    {
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(ApiEndpoints.CmsUrl.DeleteCoupon, HttpMethod.Post, requestModel);
        if (response!.IsSuccess) { return Ok(response); }
        return NotFound(response);
    }

    #endregion

    #region Member

    [HttpPost("cms/members")]
    public async Task<IActionResult> GetMembers()
    {
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(ApiEndpoints.CmsUrl.GetMembers, HttpMethod.Post, null);
        if (response!.IsSuccess) { return Ok(response); }
        return NotFound(response);
    }

    [HttpPost("cms/members/create")]
    public async Task<IActionResult> CreateMember([FromBody] ApiRequestModel requestModel)
    {
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(ApiEndpoints.CmsUrl.CreateMember, HttpMethod.Post, requestModel);
        if (response!.IsSuccess) { return Ok(response); }
        return BadRequest(response);
    }

    [HttpPost("cms/members/update")]
    public async Task<IActionResult> UpdateMember([FromBody] ApiRequestModel requestModel)
    {
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(ApiEndpoints.CmsUrl.UpdateMember, HttpMethod.Post, requestModel);
        if (response!.IsSuccess) { return Ok(response); }
        return NotFound(response);
    }

    [HttpPost("cms/members/delete")]
    public async Task<IActionResult> DeleteMember([FromBody] ApiRequestModel requestModel)
    {
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(ApiEndpoints.CmsUrl.DeleteMember, HttpMethod.Post, requestModel);
        if (response!.IsSuccess) { return Ok(response); }
        return NotFound(response);
    }

    #endregion

    #endregion

    #region Pos

    [HttpPost("pos/scan")]
    public async Task<IActionResult> ScanQr([FromBody] ApiRequestModel requestModel)
    {
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(ApiEndpoints.PosUrl.Scan, HttpMethod.Post, requestModel);
        if (response!.IsSuccess) { return Ok(response); }
        return NotFound(response);
    }

    #endregion
}