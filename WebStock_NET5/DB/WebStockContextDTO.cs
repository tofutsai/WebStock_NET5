using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebStock_NET5.DTO;

#nullable disable

namespace WebStock_NET5.DB
{
    public partial class WebStockContextDTO : WebStockContext
    {
        public WebStockContextDTO()
        {
        }

        public WebStockContextDTO(DbContextOptions<WebStockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<StockIndexDTO> StockIndexDTO { get; set; }
        public virtual DbSet<StockDataDTO> StockDataDTO { get; set; }
        public virtual DbSet<StockSysConfigDTO> StockSysConfigDTO { get; set; }
        public virtual DbSet<StockSysLogDTO> StockSysLogDTO { get; set; }
        public virtual DbSet<StockStatisticsDTO> StockStatisticsDTO { get; set; }
        public virtual DbSet<StockFavoriteDTO> StockFavoriteDTO { get; set; }
        public virtual DbSet<StockProfitDTO> StockProfitDTO { get; set; }
        public virtual DbSet<StockAvgDTO> StockAvgDTO { get; set; }
        public virtual DbSet<StockNowDTO> StockNowDTO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StockIndexDTO>().HasNoKey();
            modelBuilder.Entity<StockDataDTO>().HasNoKey();
            modelBuilder.Entity<StockSysConfigDTO>().HasNoKey();
            modelBuilder.Entity<StockSysLogDTO>().HasNoKey();
            modelBuilder.Entity<StockStatisticsDTO>().HasNoKey();
            modelBuilder.Entity<StockFavoriteDTO>().HasNoKey();
            modelBuilder.Entity<StockProfitDTO>().HasNoKey();
            modelBuilder.Entity<StockAvgDTO>().HasNoKey();
            modelBuilder.Entity<StockNowDTO>().HasNoKey();
        }
    }
}
