using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DotNet8.POS.DbService.PosDbContext;

public partial class PosDbContext : DbContext
{
    public PosDbContext()
    {
    }

    public PosDbContext(DbContextOptions<PosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCoupon> TblCoupons { get; set; }

    public virtual DbSet<TblMember> TblMembers { get; set; }

    public virtual DbSet<TblPurchasehistory> TblPurchasehistories { get; set; }

    public virtual DbSet<TblPurchasehistorydetail> TblPurchasehistorydetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<TblCoupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PRIMARY");

            entity.ToTable("tbl_coupon");

            entity.Property(e => e.CouponId).HasMaxLength(50);
            entity.Property(e => e.CouponCode).HasMaxLength(255);
            entity.Property(e => e.CouponName).HasMaxLength(255);
            entity.Property(e => e.CouponQrFilePath).HasMaxLength(100);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId).HasMaxLength(50);
            entity.Property(e => e.DelFlag).HasColumnType("bit(1)");
            entity.Property(e => e.DiscountAmount).HasPrecision(10, 2);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblMember>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_member");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId).HasMaxLength(50);
            entity.Property(e => e.DelFlag).HasColumnType("bit(1)");
            entity.Property(e => e.MemberCode).HasMaxLength(255);
            entity.Property(e => e.MemberId).HasMaxLength(50);
            entity.Property(e => e.MemberQrFilePath).HasMaxLength(100);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedUserId).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNo).HasMaxLength(15);
            entity.Property(e => e.TotalPurchasedAmount).HasPrecision(10, 2);
        });

        modelBuilder.Entity<TblPurchasehistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_purchasehistory");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId).HasMaxLength(50);
            entity.Property(e => e.MemberId).HasMaxLength(50);
            entity.Property(e => e.PurchaseHistoryId).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);
            entity.Property(e => e.TranDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblPurchasehistorydetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_purchasehistorydetail");

            entity.Property(e => e.AlcoholFree).HasColumnType("bit(1)");
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedUserId).HasMaxLength(50);
            entity.Property(e => e.ItemDescription).HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.PurchaseHistoryDetailId).HasMaxLength(50);
            entity.Property(e => e.PurchaseHistoryId).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
