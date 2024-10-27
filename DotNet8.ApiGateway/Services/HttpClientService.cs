using static System.Net.Mime.MediaTypeNames;
using DotNet8.POS.Shared;
using System.Net.Http.Headers;
using System.Text;

namespace DotNet8.POS.ApiGateway.Services;

public class HttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<T> ExecuteAsync<T>(
        string endpoint,
        object? requestModel = null
    )
    {
        HttpClient httpClient = _httpClientFactory.CreateClient("CmsService");
        HttpResponseMessage? response = null;
        HttpContent? content = null;

        if (requestModel is not null)
        {
            string json = requestModel.ToJson()!;
            content = new StringContent(json, Encoding.UTF8, Application.Json);
        }

        var getToken = _httpContextAccessor?.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == "AccessToken")?.Value;

        var accessToken = !string.IsNullOrEmpty(getToken) ? getToken : "";
        if (!string.IsNullOrEmpty(accessToken))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        response = await httpClient.PostAsync(endpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var model = responseJson.ToObject<T>();

        return model!;
    }
}