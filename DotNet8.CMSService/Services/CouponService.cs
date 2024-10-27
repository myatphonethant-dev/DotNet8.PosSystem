using DotNet8.POS.CmsService.Models;
using DotNet8.POS.DbService.PosDbContext;
using DotNet8.POS.Shared;
using Microsoft.EntityFrameworkCore;

namespace DotNet8.POS.CmsService.Services;

public class CouponService
{
    private readonly ILogger<CouponService> _logger;
    private readonly PosDbContext _context;
    private readonly CreateQrService _createQrService;

    public CouponService(PosDbContext context, ILogger<CouponService> logger, CreateQrService createQrService)
    {
        _context = context;
        _logger = logger;
        _createQrService = createQrService;
    }

    public async Task<IEnumerable<TblCoupon>> GetCoupons()
    {
        return await _context.TblCoupons.ToListAsync();
    }

    public async Task<CouponResponseModel> CreateCoupon(CouponRequestModel requestModel)
    {
        var model = new CouponResponseModel();
        try
        {
            var coupon = new TblCoupon()
            {
                CouponCode = requestModel.CouponCode,
                CouponName = requestModel.CouponName,
                DiscountAmount = requestModel.DiscountAmount,
                AvailableQuantity = requestModel.AvailableQuantity,
                StartDate = requestModel.StartDate,
                EndDate = requestModel.EndDate,
                CouponQrFilePath = await _createQrService.GenerateQrCode(requestModel.CouponCode, requestModel.CouponName, EnumQrType.Coupon),
                CreatedUserId = requestModel.UserId,
                CreatedDateTime = DateTime.UtcNow,
                DelFlag = 0
            };

            await _context.TblCoupons.AddAsync(coupon);
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Coupon Successfully Saved.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<CouponResponseModel> UpdateCoupon(CouponRequestModel requestModel)
    {
        var model = new CouponResponseModel();
        try
        {
            var coupon = await _context.TblCoupons
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.CouponCode == requestModel.CouponCode &&
                    c.DelFlag == 0);

            if (coupon is null)
            {
                model.IsSuccess = false;
                model.Message = "Coupon not found.";
                return model;
            }

            coupon.DelFlag = 1;
            coupon.ModifiedUserId = requestModel.UserId;
            coupon.ModifiedDateTime = DateTime.UtcNow;

            _context.Entry(coupon).State = EntityState.Modified;
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Coupon successfully updated.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<CouponResponseModel> DeleteCoupon(CouponRequestModel requestModel)
    {
        var model = new CouponResponseModel();
        try
        {
            var coupon = await _context.TblCoupons
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.CouponCode == requestModel.CouponCode &&
                    c.DelFlag == 0);

            if (coupon is null)
            {
                model.IsSuccess = false;
                model.Message = "Coupon not found.";
                return model;
            }

            coupon.DelFlag = 1;
            coupon.ModifiedUserId = requestModel.UserId;
            coupon.ModifiedDateTime = DateTime.UtcNow;

            _context.Entry(coupon).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            model.IsSuccess = true;
            model.Message = "Coupon successfully deleted.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }
}