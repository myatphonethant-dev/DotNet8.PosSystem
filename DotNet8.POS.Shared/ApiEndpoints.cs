namespace DotNet8.POS.Shared;

public static class ApiEndpoints
{
    public static string ApiGatewayUrl { get; } = "https://localhost:7060/api/swagger/index.html";
    public static string CacheBaseUrl { get; } = "https://localhost:7249/api/rediscache/";
    public static string CmsBaseUrl { get; } = "https://localhost:7026/api/cms/";
    public static string PointBaseUrl { get; } = "https://localhost:7181/api/point/";
    public static string PosBaseUrl { get; } = "https://localhost:7001/api/pos/";

    public static class CacheUrl
    {
        public static string UpdateCache => CacheBaseUrl + "update-cache";
    }

    public static class CmsUrl
    {
        public static string GetCoupons => CmsBaseUrl + "coupons";
        public static string CreateCoupon => CmsBaseUrl + "coupons/create";
        public static string DeleteCoupon => CmsBaseUrl + "coupons/delete";
        public static string ValidateCoupon => CmsBaseUrl + "coupons/validate-coupon";
        public static string UpdateCouponCount => CmsBaseUrl + "coupons/update-count";
        public static string GetMembers => CmsBaseUrl + "members";
        public static string CreateMember => CmsBaseUrl + "members/create";
        public static string UpdateMember => CmsBaseUrl + "members/update";
        public static string DeleteMember => CmsBaseUrl + "members/delete";
        public static string ValidateMember => CmsBaseUrl + "members/validate-member";
    }

    public static class PointUrl
    {
        public static string CalculatePoints => PointBaseUrl + "calculate";
    }

    public static class PosUrl
    {
        public static string Scan => PosBaseUrl + "scan";
    }
}