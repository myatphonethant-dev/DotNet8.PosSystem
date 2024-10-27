namespace DotNet8.POS.PosService.Services;

public class CacheService
{
    private readonly IDatabase _database;

    public CacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public int GetCacheData(string key)
    {
        var quantity = _database.StringGet(key);
        return quantity.IsNullOrEmpty ? 0 : (int)quantity;
    }

    public async Task<bool> SetCacheData(string key,string value, DateTime expires)
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