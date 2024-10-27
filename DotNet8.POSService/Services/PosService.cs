using DotNet8.POS.DbService.PosDbContext;
using DotNet8.POS.PointService.Models;
using DotNet8.POS.PosService.Models;
using DotNet8.POS.Shared;

namespace DotNet8.POS.PosService.Services;

public class PosService
{
    private readonly ILogger<PosService> _logger;
    private readonly PosDbContext _context;
    private readonly CacheService _cacheService;
    private readonly IHttpClientFactory _httpClientFactory;

    public PosService(PosDbContext context, CacheService cacheService, ILogger<PosService> logger, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> ValidateAndDecrementCouponStock(string couponCode)
    {
        #region Get Cache

        var availableQuantity = _cacheService.GetCacheData(couponCode);

        if (availableQuantity <= 0)
        {
            return false;
        }

        #endregion

        #region Update Coupon Table

        var coupon = await _context.TblCoupons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.CouponCode == couponCode);
        if (coupon == null)
        {
            return false;
        }

        coupon.AvailableQuantity--;

        _context.Entry(coupon).State = EntityState.Modified;
        await _context.SaveAndDetachAsync();

        #endregion

        #region Update Cache

        var success = _cacheService.DeleteCacheData(couponCode);
        if (!success)
        {
            return false;
        }

        #endregion

        return true;
    }

    public async Task<PointCalculationResponseModel> SendToPointSystem(PointCalculationRequestModel requestModel)
    {
        var model = new PointCalculationResponseModel();
        try
        {
            var client = _httpClientFactory.CreateClient("PointSystemClient");

            var response = await client.PostAsJsonAsync("api/points/calculate", requestModel);

            if (!response.IsSuccessStatusCode)
            {
                return new PointCalculationResponseModel
                {
                    IsSuccess = false,
                    Message = "Failed to calculate Points."
                };
            }

            var result = await response.Content.ReadFromJsonAsync<PointCalculationResponseModel>();

            model = new PointCalculationResponseModel 
            { 
                IsSuccess = false, 
                Message = "Error processing response from Point System." 
            };
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = "Something went wrong. Please try again later";
            _logger.LogCustomError(ex);
        }

        return model;
    }
}