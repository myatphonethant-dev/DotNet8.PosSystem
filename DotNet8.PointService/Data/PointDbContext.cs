namespace DotNet8.POS.PointService.Data;

public partial class PointDbContext : DbContext
{
    public PointDbContext()
    {
    }

    public PointDbContext(DbContextOptions<PointDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblPurchasehistory> TblPurchasehistories { get; set; }

    public virtual DbSet<TblPurchasehistorydetail> TblPurchasehistorydetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

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
