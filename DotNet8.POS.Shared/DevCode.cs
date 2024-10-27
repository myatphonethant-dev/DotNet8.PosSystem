namespace DotNet8.POS.Shared;

public static class DevCode
{
    #region SHA256 Password HashString

    public static string ToSHA256HexHashString(this string password, string str)
    {
        password = password.Trim();
        str = str.Trim();

        string saltedCode = EncodedBySalted(str);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.Default.GetBytes(password + saltedCode));
        var hashString = ToHex(hash, false);

        return hashString;
    }

    private static string ToHex(byte[] bytes, bool upperCase)
    {
        var result = new StringBuilder(bytes.Length * 2);
        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        return result.ToString();
    }

    private static string EncodedBySalted(string decodeString)
    {
        decodeString = decodeString.ToLower()
            .Replace("a", "@")
            .Replace("i", "!")
            .Replace("l", "1")
            .Replace("e", "3")
            .Replace("o", "0")
            .Replace("s", "$")
            .Replace("n", "&");
        return decodeString;
    }

    #endregion

    public static async Task<int> SaveAndDetachAsync(this DbContext db)
    {
        var res = await db.SaveChangesAsync();
        foreach (var entry in db.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        return res;
    }

    public static void LogCustomError(this ILogger logger,
        Exception ex,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        var fileName = Path.GetFileName(filePath);
        var message =
            $"File Name - {fileName} | Method Name - {methodName} | Result - {ex}";
        logger.LogError(message);
    }

    public static bool IsNullOrEmpty(this object? str)
    {
        var result = true;
        try
        {
            result = str == null ||
                     string.IsNullOrEmpty(str.ToString()?.Trim()) ||
                     string.IsNullOrWhiteSpace(str.ToString()?.Trim());
        }
        catch
        {
            result = false;
        }

        return result;
    }

    public static string? ToJson<T>(this T? obj, bool format = false)
    {
        if (obj == null) return string.Empty;
        string? result;
        if (obj is string)
        {
            result = obj.ToString();
            goto Result;
        }

        var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTHH:mm:ss.sssZ" };
        result = format
            ? JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, settings)
            : JsonConvert.SerializeObject(obj, settings);
    Result:
        return result;
    }

    public static T? ToObject<T>(this string? jsonStr)
    {
        try
        {
            if (jsonStr != null)
            {
                var test = JsonConvert.DeserializeObject<T>(jsonStr,
                    new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTimeOffset });
                return test;
            }
        }
        catch
        {
            return (T)Convert.ChangeType(jsonStr, typeof(T))!;
        }

        return default;
    }

    public static string UrlDecode(this string date)
    {
        return HttpUtility.UrlDecode(date);
    }

    public static string GetInternalImageUrl(this string imageGuid, string url)
    {
        Directory.CreateDirectory(url);
        return url + imageGuid;
    }
}