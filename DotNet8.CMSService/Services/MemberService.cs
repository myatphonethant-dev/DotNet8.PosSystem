using DotNet8.POS.Shared.Models.Cms;
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

    public async Task<MemberResponseModel> GetMembers()
    {
        var model = new MemberResponseModel();
        var data = await _context.TblMembers
                    .AsNoTracking()
                    .Where(x => x.DelFlag == 0)
                    .ToListAsync();

        model.Data = data.Select(c => new MemberModel
        {
            MemberCode = c.MemberCode,
            Name = c.Name,
            PhoneNo = c.PhoneNo,
            TotalPoints = c.TotalPoints,
            TotalPurchasedAmount = c.TotalPurchasedAmount,
            MemberQrFilePath = c.MemberQrFilePath,
            CreatedUserId = c.CreatedUserId,
            CreatedDateTime = c.CreatedDateTime,
        }).ToList();
        model.IsSuccess = true;
        model.Message = "Successfully Retrieved Data.";
        return model;
    }

    public async Task<MemberResponseModel> CreateMember(CreateMemberRequestModel requestModel)
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

    public async Task<MemberResponseModel> UpdateMember(UpdateMemberRequestModel requestModel)
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

    public async Task<MemberResponseModel> DeleteMember(DeleteMemberRequestModel requestModel)
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