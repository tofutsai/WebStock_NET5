using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class WebStockContext : DbContext
    {
        public WebStockContext()
        {
        }

        public WebStockContext(DbContextOptions<WebStockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<member> member { get; set; }
        public virtual DbSet<stockAvg> stockAvg { get; set; }
        public virtual DbSet<stockDataTmp> stockDataTmp { get; set; }
        public virtual DbSet<stockDataTmpOtc> stockDataTmpOtc { get; set; }
        public virtual DbSet<stockData> stockData { get; set; }
        public virtual DbSet<stockFavorite> stockFavorite { get; set; }
        public virtual DbSet<stockIndex> stockIndex { get; set; }
        public virtual DbSet<stockMemo> stockMemo { get; set; }
        public virtual DbSet<stockNow> stockNow { get; set; }
        public virtual DbSet<stockProfit> stockProfit { get; set; }
        public virtual DbSet<sysConfig> sysConfig { get; set; }
        public virtual DbSet<sysLog> sysLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<member>(entity =>
            {
                entity.ToTable("member");

                entity.Property(e => e.account)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.role)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<stockAvg>(entity =>
            {
                entity.ToTable("stockAvg");

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<stockDataTmp>(entity =>
            {
                entity.ToTable("stockDataTmp");

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.dataDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<stockDataTmpOtc>(entity =>
            {
                entity.ToTable("stockDataTmpOtc");

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.dataDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<stockData>(entity =>
            {
                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.dataDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<stockFavorite>(entity =>
            {
                entity.ToTable("stockFavorite");

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.memo).HasMaxLength(50);
            });

            modelBuilder.Entity<stockIndex>(entity =>
            {
                entity.ToTable("stockIndex");

                entity.Property(e => e.category)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.company)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.dataDate).HasColumnType("datetime");

                entity.Property(e => e.type)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<stockMemo>(entity =>
            {
                entity.ToTable("stockMemo");

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ext1).HasMaxLength(50);

                entity.Property(e => e.ext10).HasMaxLength(50);

                entity.Property(e => e.ext11).HasMaxLength(50);

                entity.Property(e => e.ext12).HasMaxLength(50);

                entity.Property(e => e.ext13).HasMaxLength(50);

                entity.Property(e => e.ext14).HasMaxLength(50);

                entity.Property(e => e.ext15).HasMaxLength(50);

                entity.Property(e => e.ext16).HasMaxLength(50);

                entity.Property(e => e.ext2).HasMaxLength(50);

                entity.Property(e => e.ext3).HasMaxLength(50);

                entity.Property(e => e.ext4).HasMaxLength(50);

                entity.Property(e => e.ext5).HasMaxLength(50);

                entity.Property(e => e.ext6).HasMaxLength(50);

                entity.Property(e => e.ext7).HasMaxLength(50);

                entity.Property(e => e.ext8).HasMaxLength(50);

                entity.Property(e => e.ext9).HasMaxLength(50);
            });

            modelBuilder.Entity<stockNow>(entity =>
            {
                entity.ToTable("stockNow");

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.dataDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<stockProfit>(entity =>
            {
                entity.ToTable("stockProfit");

                entity.Property(e => e.code)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<sysConfig>(entity =>
            {
                entity.ToTable("sysConfig");

                entity.Property(e => e.avgEndDate).HasColumnType("datetime");

                entity.Property(e => e.avgStartDate).HasColumnType("datetime");

                entity.Property(e => e.nowDate).HasColumnType("datetime");

                entity.Property(e => e.otcUpdate).HasColumnType("datetime");

                entity.Property(e => e.stockUpdate).HasColumnType("datetime");
            });

            modelBuilder.Entity<sysLog>(entity =>
            {
                entity.ToTable("sysLog");

                entity.Property(e => e.date).HasColumnType("datetime");

                entity.Property(e => e.message).IsRequired();

                entity.Property(e => e.type)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
