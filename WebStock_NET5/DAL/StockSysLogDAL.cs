using Microsoft.Data.SqlClient;
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
    public interface IStockSysLogDAL
    {
        List<StockSysLogDTO> Read(FormSearch f);
        public sysLog Create(string type, string message);
    }

    public class StockSysLogDAL : IStockSysLogDAL
    {
        private readonly WebStockContextDTO _db;
        public StockSysLogDAL(WebStockContextDTO webStockContextDTO)
        {
            _db = webStockContextDTO;
        }
        public List<StockSysLogDTO> Read(FormSearch f)
        {
            string datefrom = f.dataDate.ToString("yyyy-MM-dd");
            string datenow = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");

            string sql = @"SELECT
                               COUNT(1) OVER () AS totalCount
                               ,*
                               FROM sysLog AS s
                               WHERE s.date BETWEEN @datefrom AND @datenow
                               ORDER BY s.id DESC {0}"
                               ;
            string pageStr = (f.options.page != 0 && f.options.itemsPerPage != 0) ? "OFFSET @OFFSET ROWS FETCH NEXT @FETCH ROWS ONLY" : "";
            string strsql = string.Format(sql, pageStr);

            List<StockSysLogDTO> rs = _db.StockSysLogDTO.FromSqlRaw(strsql,
                new SqlParameter("@datefrom", datefrom),
                new SqlParameter("@datenow", datenow),
                new SqlParameter("@OFFSET", ((f.options.page - 1) * f.options.itemsPerPage)),
                new SqlParameter("@FETCH", f.options.itemsPerPage)).ToList();

            if (rs.Count() == 0)
            {
                return new List<StockSysLogDTO>();
            }
            return rs;
        }

        public sysLog Create(string type, string message)
        {
            sysLog log = new sysLog();
            log.date = DateTime.UtcNow.AddHours(08);
            log.type = type;
            log.message = message;
            _db.sysLog.Add(log);
            _db.SaveChanges();
            return log;
        }
    }
}
