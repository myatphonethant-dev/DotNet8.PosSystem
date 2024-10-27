using DotNet8.POS.DbService.PosDbContext;
using DotNet8.POS.PointService.Models;
using DotNet8.POS.Shared;
using Microsoft.EntityFrameworkCore;

namespace DotNet8.POS.PointService.Services;

public class PointService
{
    private readonly ILogger<PointService> _logger;
    private readonly PosDbContext _context;

    public PointService(PosDbContext context, ILogger<PointService> logger)
    {
        _context = context;
        _logger = logger;
    }

    //public async Task<PointCalculationResponseModel> CalculatePoints(PointCalculationRequestModel requestModel)
    //{
    //    var response = new PointCalculationResponseModel();

    //    try
    //    {
    //        var totalPoints = requestModel.PurchasedItems
    //            .Where(item => item.ItemDescription != "Alcohol")
    //            .Sum(item => (int)(item.TotalPrice / 10));

    //        response.EarnedPoints = totalPoints;
    //        response.IsSuccess = true;
    //        response.Message = $"Total {totalPoints} points earned.";

    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogCustomError(ex);
    //        response.IsSuccess = false;
    //        response.Message = "Error calculating points. Please try again.";
    //    }
    //    return response;
    //}

    public async Task<bool> UpdateMemberPoints(string memberCode, int points)
    {
        try
        {
            var member = await _context.TblMembers
                .FirstOrDefaultAsync(m => m.MemberCode == memberCode);

            if (member == null)
            {
                return false;
            }

            member.TotalPoints += points;
            _context.Entry(member).State = EntityState.Modified;

            await _context.SaveAndDetachAsync();
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return false;
        }
        return true;
    }

    public async Task<PointCalculationResponseModel> CalculatePoints(PointCalculationRequestModel requestModel)
    {
        var model = new PointCalculationResponseModel();
        try
        {
            #region Validate Member

            var member = await _context.TblMembers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m =>
                            m.MemberId == requestModel.MemberId &&
                            m.MemberCode == requestModel.MemberCode);
            if (member == null)
            {
                model.IsSuccess = false;
                model.Message = "Member not found!";
                return model;
            }

            #endregion

            using (var transaction = _context.Database.BeginTransaction())
            {
                #region Update Member Point

                var totalPrice = requestModel.PurchasedItems
                .Where(i => i.ItemDescription == "Non Alcohol")
                .Sum(i => i.TotalPrice);

                var totalPoints = (int)(totalPrice / 10);

                member!.TotalPoints += totalPoints;
                member.TotalPurchasedAmount += totalPrice;

                _context.Entry(member).State = EntityState.Modified;
                await _context.SaveAndDetachAsync();

                #endregion

                #region Save Purchase History

                var history = new PurchaseHistoryModel()
                {
                    PurchaseHistoryId = Guid.NewGuid().ToString(),
                    MemberId = member.MemberId,
                    TotalPoint = totalPoints,
                    TotalPrice = totalPrice,
                    TranDate = DateTime.UtcNow,
                    CreatedUserId = requestModel.UserId,
                    CreatedDateTime = DateTime.UtcNow,
                };
                await _context.AddAsync(history);

                #endregion

                #region Save Purchase History Detail

                var historyList = new List<PurchaseHistoryDetailModel>();

                foreach (var item in requestModel.PurchasedItems)
                {
                    var historyItem = new PurchaseHistoryDetailModel()
                    {
                        PurchaseHistoryDetailId = Guid.NewGuid().ToString(),
                        PurchaseHistoryId = history.PurchaseHistoryId,
                        ItemDescription = item.ItemDescription,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        TotalPrice = item.Price * item.Quantity,
                        CreatedUserId = requestModel.UserId,
                        CreatedDateTime = DateTime.UtcNow,
                    };
                    await _context.AddAsync(historyItem);
                }

                #endregion

                await _context.SaveAndDetachAsync();

                await transaction.CommitAsync();

                model.IsSuccess = true;
                model.EarnedPoints = totalPoints;
                model.Message = $"Total {totalPoints} earned!";
            }
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
