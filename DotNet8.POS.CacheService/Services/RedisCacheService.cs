namespace DotNet8.POS.RedisCacheService.Services;

public class RedisCacheService
{
    private readonly IDatabase _database;
    private readonly HttpClientService _httpClient;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, HttpClientService httpClient)
    {
        _database = connectionMultiplexer.GetDatabase();
        _httpClient = httpClient;
    }

    public async Task<bool> UpdateCache(string couponCode)
    {
        #region Get Cache

        var availableQuantity = GetCacheData(couponCode);

        if (availableQuantity <= 0)
        {
            return false;
        }

        #endregion

        #region PassToCms

        var endpoint = $"http://posservice/api/coupons/update-count?couponCode={couponCode}";
        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(endpoint, HttpMethod.Post, couponCode);
        if (response!.IsSuccess == false)
        {
            return false;
        }

        #endregion

        #region Update Cache

        var success = DeleteCacheData(couponCode);
        if (!success)
        {
            return false;
        }

        #endregion

        return true;
    }

    public int GetCacheData(string key)
    {
        var quantity = _database.StringGet(key);
        return quantity.IsNullOrEmpty ? 0 : (int)quantity;
    }

    public async Task<bool> SetCacheData(string key, string value, DateTime expires)
    {
        var expiredTime = expires.Subtract(DateTime.Now);
        return await _database.StringSetAsync(key, value, expiredTime);
    }

    public bool DeleteCacheData(string key)
    {
        var exist = _database.KeyExists(key);
        if (exist)
        {
            return _database.KeyDelete(key);
        }
        return false;
    }
}