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
    public interface IStockStatisticsDAL
    {
        List<StockStatisticsDTO> Read(FormSearch f);

    }
    public class StockStatisticsDAL : IStockStatisticsDAL
    {
        private readonly WebStockContextDTO _db;

        public StockStatisticsDAL(WebStockContextDTO WebStockContextDTO)
        {
            _db = WebStockContextDTO;
        }

        public List<StockStatisticsDTO> Read(FormSearch f)
        {
            string strSqlTmp = @"
                                    SELECT
                                    count(1) over() as totalCount, 
                                    i.type
                                   ,i.category
                                   ,i.code 
                                   ,i.company 
                                   ,a.avgShares 
                                   ,a.avgTurnover 
                                   ,a.highestPrice 
                                   ,a.lowestPrice 
                                   ,n.closePrice 
                                   ,n.position
                                   ,n.dataDate
                                   ,ISNULL(
	                                m.ext1 + ',' +
	                                m.ext2 + ',' +
	                                m.ext3 + ',' +
	                                m.ext4 + ',' +
	                                m.ext5 + ',' +
	                                m.ext6 + ',' +
	                                m.ext7
	                                , '') AS memo
                                    FROM stockIndex i
                                    JOIN stockAvg a
                                    ON i.code = a.code
                                    JOIN stockNow n
                                    ON i.code = n.code
                                    LEFT JOIN stockMemo m
	                                ON m.code = i.code";

            if (string.IsNullOrEmpty(f.code))
            {
                strSqlTmp += @" WHERE i.type LIKE '%'+ @type +'%' AND a.avgShares >= @shares AND n.position <= @position AND
                                    n.closePrice >= @closePrice {0} {1} {2} ";
                string sortStr = "ORDER BY ";
                string sortDESC = (f.options.sortDesc != null && f.options.sortDesc.Count() > 0 && f.options.sortDesc[0]) ? "DESC" : "ASC";
                string pageStr = (f.options.page != 0 && f.options.itemsPerPage != 0) ? "OFFSET @OFFSET ROWS FETCH NEXT @FETCH ROWS ONLY" : "";

                string sortByType = (f.options.sortBy != null && f.options.sortBy.Count() > 0 && f.options.sortBy[0] != "") ? f.options.sortBy[0] : "";
                switch (sortByType)
                {
                    case "type": sortStr += "i.type"; break;
                    case "code": sortStr += "i.code"; break;
                    case "company": sortStr += "i.company"; break;
                    case "position": sortStr += "n.position"; break;
                    case "category": sortStr += "i.category"; break;
                    case "avgShares": sortStr += "a.avgShares"; break;
                    case "avgTurnover": sortStr += "a.avgTurnover"; break;
                    case "highestPrice": sortStr += "a.highestPrice"; break;
                    case "lowestPrice": sortStr += "a.lowestPrice"; break;
                    case "closePrice": sortStr += "n.closePrice"; break;
                    default: sortStr += "i.dataDate"; break;
                }

                string strSql = string.Format(strSqlTmp, sortStr, sortDESC, pageStr);

                List<StockStatisticsDTO> rs = _db.StockStatisticsDTO.FromSqlRaw(strSql,
                    new SqlParameter("@type", f.type),
                    new SqlParameter("@shares", f.shares),
                    new SqlParameter("@position", f.position2),
                    new SqlParameter("@closePrice", f.closePrice),
                    new SqlParameter("@OFFSET", ((f.options.page - 1) * f.options.itemsPerPage)),
                    new SqlParameter("@FETCH", f.options.itemsPerPage)).ToList();
                if(rs.Count == 0)
                {
                    return new List<StockStatisticsDTO>();
                }
                return rs;
            }
            else
            {
                strSqlTmp += @" WHERE ( a.code LIKE @code OR i.company LIKE @code )";
                List<StockStatisticsDTO> rs = _db.StockStatisticsDTO.FromSqlRaw(strSqlTmp,
                    new SqlParameter("@code", f.code)).ToList();
                if (rs.Count == 0)
                {
                    return new List<StockStatisticsDTO>();
                }
                return rs;
            }
        }
    }
}
