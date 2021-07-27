using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStock_NET5.DB;
using WebStock_NET5.DTO;
using static WebStock_NET5.Common;

namespace WebStock_NET5.DAL
{
    public interface IStockSysConfigDAL
    {
        List<StockSysConfigDTO> Read();
        bool Edit(sysConfig f);
        public DateTime GetsysConfigstockUpdate();
        public DateTime GetsysConfigotcUpdate();
        public int EditsysConfigstockUpdate(DateTime nextdate);
        public int EditsysConfigotcUpdate(DateTime nextdate);
    }

    public class StockSysConfigDAL : IStockSysConfigDAL
    {
        private readonly WebStockContextDTO _db;
        public StockSysConfigDAL(WebStockContextDTO WebStockContextDTO)
        {
            _db = WebStockContextDTO;
        }
        public List<StockSysConfigDTO> Read()
        {
            string sql = @"SELECT
                               COUNT(1) OVER () AS totalCount
                               ,*
                               FROM sysConfig";

            List<StockSysConfigDTO> rs = _db.StockSysConfigDTO.FromSqlRaw(sql).ToList();

            return rs;
        }

        public bool Edit(sysConfig f)
        {
            int status = 0;

            var stockSysConfig = _db.sysConfig.Where(x => x.id == f.id).FirstOrDefault();
            Mapper.Initialize(cfg => cfg.CreateMap<sysConfig, sysConfig>()
                                        .ForMember(x => x.id, opt => opt.Ignore()));
            Mapper.Map(f, stockSysConfig);
            status = _db.SaveChanges();
            return status >= 0 ? true : false;
        }

        public DateTime GetsysConfigstockUpdate()
        {
            DateTime stockdate = _db.sysConfig.Select(m => m.stockUpdate).FirstOrDefault();
            return stockdate;
        }

        public DateTime GetsysConfigotcUpdate()
        {
            DateTime otcdate = _db.sysConfig.Select(m => m.otcUpdate).FirstOrDefault();
            return otcdate;
        }

        public int EditsysConfigstockUpdate(DateTime nextdate)
        {
            var res = _db.sysConfig.Where(x => x.id == 1).FirstOrDefault();
            res.stockUpdate = nextdate;

            return _db.SaveChanges();
        }

        public int EditsysConfigotcUpdate(DateTime nextdate)
        {
            var res = _db.sysConfig.Where(x => x.id == 1).FirstOrDefault();
            res.otcUpdate = nextdate;

            return _db.SaveChanges();
        }
    }

}

