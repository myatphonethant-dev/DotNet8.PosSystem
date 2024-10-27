namespace DotNet8.POS.Shared;

public class HttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<T?> ExecuteAsync<T>(
        string endpoint,
        HttpMethod method,
        object? requestModel = null
    )
    {
        var httpClient = _httpClientFactory.CreateClient();
        HttpRequestMessage request = new HttpRequestMessage(method, endpoint);

        if (requestModel is not null)
        {
            var json = requestModel.ToJson();
            request.Content = new StringContent(json!, Encoding.UTF8, "application/json");
        }

        var accessToken = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == "AccessToken")?.Value;

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request to {endpoint} failed with status code {response.StatusCode}. Error: {errorMessage}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        return responseJson.ToObject<T>();
    }
}
