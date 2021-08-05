using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebStock_NET5.Common;
using WebStock_NET5.DB;
using WebStock_NET5.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace WebStock_NET5.DAL
{
    public interface IStockProfitDAL
    {
        List<StockProfitDTO> Read(FormSearch f);
        bool Create(stockProfit f);
        //bool Edit(stockProfit f);
        bool Delete(int id);

    }
    public class StockProfitDAL : IStockProfitDAL
    {
        private readonly WebStockContextDTO _db;
        public StockProfitDAL(WebStockContextDTO webStockContextDTO)
        {
            _db = webStockContextDTO;
        }
        public List<StockProfitDTO> Read(FormSearch f)
        {
            string sql = @"SELECT
                             count(1) over() as totalCount,
                             p.id
                            ,i.code
                            ,i.company
                            ,n.position
                            ,n.closePrice
                            ,p.buyPrice
                            ,p.buyShares
                            ,p.buyCost
                            ,p.profit
                            ,p.profitPercentage
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
                            JOIN stockNow n
                            	ON i.code = n.code
                            JOIN stockProfit p
                            	ON i.code = p.code
                            LEFT JOIN stockMemo m
                               ON p.code = m.code
                            WHERE p.operId = @operId
                            {0} {1} {2}";
            string sortStr = "ORDER BY ";
            string sortDESC = (f.options.sortDesc != null && f.options.sortDesc.Count() > 0 && f.options.sortDesc[0]) ? "DESC" : "ASC";
            string pageStr = (f.options.page != 0 && f.options.itemsPerPage != 0) ? "OFFSET @OFFSET ROWS FETCH NEXT @FETCH ROWS ONLY" : "";
            string sortByType = (f.options.sortBy != null && f.options.sortBy.Count() > 0 && f.options.sortBy[0] != "") ? f.options.sortBy[0] : "";
            switch (sortByType)
            {
                case "code": sortStr += "i.code"; break;
                case "company": sortStr += "i.company"; break;
                case "position": sortStr += "n.position"; break;
                case "closePrice": sortStr += "n.closePrice"; break;
                case "buyPrice": sortStr += "p.buyPrice"; break;
                case "buyShares": sortStr += "p.buyShares"; break;
                case "buyCost": sortStr += "p.buyCost"; break;
                case "profit": sortStr += "p.profit"; break;
                case "profitPercentage": sortStr += "p.profitPercentage"; break;
                default: sortStr += "i.code"; break;
            }
            string strsql = string.Format(sql, sortStr, sortDESC, pageStr);
            List<StockProfitDTO> rs = _db.StockProfitDTO.FromSqlRaw(strsql,
                                      new SqlParameter("@operId", f.operId),
                                      new SqlParameter("@OFFSET", ((f.options.page - 1) * f.options.itemsPerPage)),
                                      new SqlParameter("@FETCH", f.options.itemsPerPage)).ToList();
            if (rs.Count == 0)
            {
                return new List<StockProfitDTO>();
            }

            const double sellcommision = 0.004425;
            const int percentage = 100;
            foreach (var item in rs)
            {
                item.profit = ((item.closePrice * item.buyShares * (1 - sellcommision)) - item.buyCost);
                item.profitPercentage = Math.Round(item.profit / item.buyCost * percentage, 2);
            }

            return rs;
        }

        public bool Create(stockProfit f)
        {
            int status = 0;

            stockIndex company = _db.stockIndex.Where(x => x.company == f.code).FirstOrDefault();
            if (company != null)
                f.code = company.code;
            const double buycommision = 1.001425;
            const double sellcommision = 0.004425;
            const int percentage = 100;
            stockProfit stockProfit = new stockProfit();
            stockNow stockNow = _db.stockNow.Where(x => x.code == f.code).FirstOrDefault();
            if (stockNow == null)
                return false;
            stockProfit.operId = f.operId;
            stockProfit.code = f.code;
            stockProfit.buyPrice = f.buyPrice;
            stockProfit.buyShares = f.buyShares;
            stockProfit.buyCost = (f.buyPrice * f.buyShares * buycommision);
            stockProfit.profit = (stockNow.closePrice * f.buyShares * (1 - sellcommision) - (f.buyPrice * f.buyShares * buycommision));
            stockProfit.profitPercentage = Math.Round((stockProfit.profit / stockProfit.buyCost) * percentage, 2);
            _db.stockProfit.Add(stockProfit);
            status = _db.SaveChanges();

            return status > 0;
        }

        public bool Delete(int id)
        {
            int status = 0;
            var del = _db.stockProfit.Where(x => x.id == id).FirstOrDefault();
            _db.stockProfit.Remove(del);
            status = _db.SaveChanges();

            return status > 0;
        }
    }
}
