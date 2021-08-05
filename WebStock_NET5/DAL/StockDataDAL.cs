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
    public interface IStockDataDAL
    {
        List<StockDataDTO> Read(FormSearch f);

    }
    public class StockDataDAL : IStockDataDAL
    {
        private readonly WebStockContextDTO _db;
        public StockDataDAL(WebStockContextDTO WebStockContextDTO)
        {
            _db = WebStockContextDTO;
        }

        public List<StockDataDTO> Read(FormSearch f)
        {
            string strSqlTmp = @"
                                    SELECT
                                    count(1) over() as totalCount, 
                                    a.id,			
                                    a.code,		
                                    b.company,
                                    a.dataDate,	
                                    a.shares,		
                                    a.turnover,	
                                    a.openPrice,
                                    a.highestPrice,
                                    a.lowestPrice,	
                                    a.closePrice
                                    FROM stockData a
                                    JOIN stockIndex b
                                    ON a.code = b.code
                                    WHERE
                                    (
                                    a.code LIKE @code
                                    OR
                                    b.company LIKE @code
                                    )
                                    AND
                                    (
                                    a.dataDate >= @dataDate
                                    )
                                    AND
                                    (
                                    b.isEnable = 'true'
                                    )
                                    {0} {1}
                                    {2}
                                    ";
            string sortStr = "ORDER BY ";
            string sortDESC = (f.options.sortDesc != null && f.options.sortDesc.Count() > 0 && f.options.sortDesc[0]) ? "DESC" : "ASC";
            string pageStr = (f.options.page != 0 && f.options.itemsPerPage != 0) ? "OFFSET @OFFSET ROWS FETCH NEXT @FETCH ROWS ONLY" : "";

            string sortByType = (f.options.sortBy != null && f.options.sortBy.Count() > 0 && f.options.sortBy[0] != "") ? f.options.sortBy[0] : "";
            switch (sortByType)
            {
                case "id": sortStr += "a.id"; break;
                case "code": sortStr += "a.code"; break;
                case "company": sortStr += "b.company"; break;
                case "dataDate": sortStr += "a.dataDate"; break;
                case "shares": sortStr += "a.shares"; break;
                case "turnover": sortStr += "a.turnover"; break;
                case "openPrice": sortStr += "a.openPrice"; break;
                case "highestPrice": sortStr += "a.highestPrice"; break;
                case "lowestPrice": sortStr += "a.lowestPrice"; break;
                case "closePrice": sortStr += "a.closePrice"; break;
                default: sortStr += "a.dataDate"; break;
            }

            string strSql = string.Format(strSqlTmp, sortStr, sortDESC, pageStr);

            List<StockDataDTO> rs = _db.StockDataDTO.FromSqlRaw(strSql,
                    new SqlParameter("@code", f.code ?? string.Empty),
                    new SqlParameter("@dataDate", f.dataDate),
                    new SqlParameter("@OFFSET", ((f.options.page - 1) * f.options.itemsPerPage)),
                    new SqlParameter("@FETCH", f.options.itemsPerPage)).ToList();

            if(rs.Count() == 0)
            {
                return new List<StockDataDTO>();
            }

            return rs;
        }
    }
}
