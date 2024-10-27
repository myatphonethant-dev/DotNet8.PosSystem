using DotNet8.POS.Shared;
using System.Net.Http.Headers;

namespace DotNet8.POS.ApiGateway.Services;

public class AddAccessTokenHeader : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AddAccessTokenHeader> _logger;

    public AddAccessTokenHeader(IHttpContextAccessor httpContextAccessor, ILogger<AddAccessTokenHeader> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var getToken = _httpContextAccessor?.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "AccessToken")?.Value;
            var accessToken = !getToken.IsNullOrEmpty() ? getToken : "";
            if (!accessToken.IsNullOrEmpty())
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            _logger.LogInformation($"Access Token : {accessToken}");
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.ToString());
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
