namespace DotNet8.POS.PointService.Services;

public class PointService
{
    private readonly ILogger<PointService> _logger;
    private readonly HttpClientService _httpClient;
    private readonly PointDbContext _context;

    public PointService(ILogger<PointService> logger, HttpClientService httpClient, PointDbContext context)
    {
        _logger = logger;
        _httpClient = httpClient;
        _context = context;
    }

    public async Task<PointExchangeResponseModel> CalculatePoints(PointExchangeRequestModel requestModel)
    {
        var model = new PointExchangeResponseModel();

        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
                {
                    #region Update Member Point

                    var totalPrice = requestModel.PurchasedItems
                    .Where(i => i.ItemDescription == "Non Alcohol")
                    .Sum(i => i.TotalPrice);

                    var totalPoints = (int)(totalPrice / 10);

                    var request = new UpdatePointModel
                    {
                        MemberId = requestModel.MemberId,
                        MemberCode = requestModel.MemberCode,
                        TotalPoint = totalPoints,
                        TotalPrice = totalPrice
                    };

                    var response = await PassToCms(request);

                    #endregion

                    #region Save Purchase History

                    var history = new TblPurchasehistory()
                    {
                        PurchaseHistoryId = Guid.NewGuid().ToString(),
                        MemberId = requestModel.MemberId,
                        TotalPoint = totalPoints,
                        TotalPrice = totalPrice,
                        TranDate = DateTime.UtcNow,
                        CreatedUserId = requestModel.UserId,
                        CreatedDateTime = DateTime.UtcNow,
                    };
                    await _context.AddAsync(history);

                    #endregion

                    #region Save Purchase History Detail

                    var historyList = new List<TblPurchasehistorydetail>();

                    foreach (var item in requestModel.PurchasedItems)
                    {
                        var historyItem = new TblPurchasehistorydetail()
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
                    model.Message = $"Total {totalPoints} points earned!";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    model.IsSuccess = false;
                    model.Message = "Something went wrong. Please try again later";
                    _logger.LogCustomError(ex);
                }
            }
        });

        return model;
    }

    private async Task<bool> PassToCms(UpdatePointModel requestModel)
    {
        var endpoint = ApiEndpoints.CmsUrl.UpdateCouponCount;

        var response = await _httpClient.ExecuteAsync<ApiResponseModel>(endpoint, HttpMethod.Post, requestModel);
        if (response!.IsSuccess == false)
        {
            return false;
        }
        return true;
    }
}