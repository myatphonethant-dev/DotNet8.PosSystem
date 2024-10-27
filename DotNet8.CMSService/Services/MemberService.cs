using DotNet8.POS.CmsService.Models;
using DotNet8.POS.DbService.PosDbContext;
using DotNet8.POS.Shared;
using Microsoft.EntityFrameworkCore;

namespace DotNet8.POS.CmsService.Services;

public class MemberService
{
    private readonly ILogger<MemberService> _logger;
    private readonly PosDbContext _context;
    private readonly CreateQrService _createQrService;

    public MemberService(PosDbContext context, ILogger<MemberService> logger, CreateQrService createQrService)
    {
        _context = context;
        _logger = logger;
        _createQrService = createQrService;
    }

    public async Task<IEnumerable<TblMember>> GetMembers()
    {
        return await _context.TblMembers.ToListAsync();
    }

    public async Task<MemberResponseModel> CreateCoupon(MemberRequestModel requestModel)
    {
        var model = new MemberResponseModel();
        try
        {
            var member = new TblMember()
            {
                MemberId = Guid.NewGuid().ToString(),
                MemberCode = requestModel.MemberCode,
                Name = requestModel.Name,
                PhoneNo = requestModel.PhoneNo,
                TotalPoints = 0,
                TotalPurchasedAmount = 0,
                MemberQrFilePath = await _createQrService.GenerateQrCode(requestModel.MemberCode, requestModel.Name, EnumQrType.Member),
                CreatedUserId = requestModel.UserId,
                CreatedDateTime = DateTime.UtcNow,
                DelFlag = 0
            };

            await _context.TblMembers.AddAsync(member);
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

    public async Task<MemberResponseModel> UpdateMember(MemberRequestModel requestModel)
    {
        var model = new MemberResponseModel();
        try
        {
            var member = await _context.TblMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(m =>
                    m.MemberCode == requestModel.MemberCode &&
                    m.DelFlag == 0);

            if (member is null)
            {
                model.IsSuccess = false;
                model.Message = "Member not found.";
                return model;
            }

            member.PhoneNo = requestModel.PhoneNo;
            member.ModifiedUserId = requestModel.UserId;
            member.ModifiedDateTime = DateTime.UtcNow;

            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Member successfully updated.";
        }
        catch (Exception ex)
        {
            model.IsSuccess = false;
            model.Message = ex.Message;
            _logger.LogCustomError(ex);
        }
        return model;
    }

    public async Task<MemberResponseModel> DeleteMember(MemberRequestModel requestModel)
    {
        var model = new MemberResponseModel();
        try
        {
            var member = await _context.TblMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(m =>
                    m.MemberCode == requestModel.MemberCode &&
                    m.DelFlag == 0);

            if (member is null)
            {
                model.IsSuccess = false;
                model.Message = "Member not found.";
                return model;
            }

            member.DelFlag = 1;
            member.ModifiedUserId = requestModel.UserId;
            member.ModifiedDateTime = DateTime.UtcNow;

            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveAndDetachAsync();

            model.IsSuccess = true;
            model.Message = "Member successfully deleted.";
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