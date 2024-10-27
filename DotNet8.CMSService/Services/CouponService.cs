using DotNet8.POS.Shared.Models.Cms;
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

    public async Task<CouponResponseModel> GetCoupons()
    {
        var model = new CouponResponseModel();
        var data = await _context.TblCoupons
                   .AsNoTracking()
                   .Where(x => x.DelFlag == 0)
                   .ToListAsync();

        model.Data = data.Select(c => new CouponModel
        {
            CouponCode = c.CouponCode,
            CouponName = c.CouponName,
            DiscountAmount = c.DiscountAmount,
            AvailableQuantity = c.AvailableQuantity,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            CouponQrFilePath = c.CouponQrFilePath
        }).ToList();
        model.IsSuccess = true;
        model.Message = "Successfully Retrieved Data.";
        return model;
    }

    public async Task<CouponResponseModel> CreateCoupon(CreateCouponRequestModel requestModel)
    {
        var model = new CouponResponseModel();
        try
        {
            var item = await _context.TblCoupons
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.CouponCode == requestModel.CouponCode &&
                    c.DelFlag == 0);

            if (item is not null)
            {
                model.IsSuccess = false;
                model.Message = "Coupon already exist.";
                return model;
            }

            var coupon = new TblCoupon()
            {
                CouponId = Guid.NewGuid().ToString(),
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

    public async Task<CouponResponseModel> DeleteCoupon(DeleteCouponRequestModel requestModel)
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