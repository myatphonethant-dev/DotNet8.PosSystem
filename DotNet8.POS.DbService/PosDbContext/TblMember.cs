using System;
using System.Collections.Generic;

namespace DotNet8.POS.DbService.PosDbContext;

public partial class TblMember
{
    public string? MemberId { get; set; }

    public string? MemberCode { get; set; }

    public string? Name { get; set; }

    public string? PhoneNo { get; set; }

    public int? TotalPoints { get; set; }

    public decimal? TotalPurchasedAmount { get; set; }

    public string? MemberQrFilePath { get; set; }

    public string CreatedUserId { get; set; } = null!;

    public DateTime CreatedDateTime { get; set; }

    public string? ModifiedUserId { get; set; }

    public DateTime? ModifiedDateTime { get; set; }

    public ulong DelFlag { get; set; }
}
