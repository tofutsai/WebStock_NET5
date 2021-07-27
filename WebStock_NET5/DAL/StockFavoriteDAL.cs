using AutoMapper;
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
    public interface IStockFavoriteDAL
    {
        List<StockFavoriteDTO> Read(FormSearch f);
        string Create(stockFavorite f);
        bool Edit(stockFavorite f);
        bool Delete(int id);

    }
    public class StockFavoriteDAL : IStockFavoriteDAL
    {
        private readonly WebStockContextDTO _db;
        public StockFavoriteDAL(WebStockContextDTO WebStockContextDTO)
        {
            _db = WebStockContextDTO;
        }
        public List<StockFavoriteDTO> Read(FormSearch f)
        {
            string sql = @"SELECT
                          count(1) over() as totalCount,
                          i.type
                         ,i.category
                         ,i.code 
                         ,i.company
                         ,f.id
                         ,n.closePrice 
                         ,n.position
                         ,ISNULL(
                          m.ext1 + ',' +
                          m.ext2 + ',' +
                          m.ext3 + ',' +
                          m.ext4 + ',' +
                          m.ext5 + ',' +
                          m.ext6 + ',' +
                          m.ext7
                          , '') AS memo
                         ,f.memo AS selfmemo
                         FROM stockIndex i
                         JOIN stockNow n
                         	ON i.code = n.code
                         JOIN stockFavorite f
                         	ON i.code = f.code
                         LEFT JOIN stockMemo m
                         ON f.code = m.code
                         WHERE f.operId = @operId
                         {0} {1} {2}";
            string sortStr = "ORDER BY ";
            string sortDESC = (f.options.sortDesc != null && f.options.sortDesc[0]) ? "DESC" : "ASC";
            string pageStr = (f.options.page != 0 && f.options.itemsPerPage != 0) ? "OFFSET @OFFSET ROWS FETCH NEXT @FETCH ROWS ONLY" : "";
            string sortByType = (f.options.sortBy != null && f.options.sortBy[0] != "") ? f.options.sortBy[0] : "";
            switch (sortByType)
            {
                case "type": sortStr += "i.type"; break;
                case "code": sortStr += "i.code"; break;
                case "company": sortStr += "i.company"; break;
                case "position": sortStr += "n.position"; break;
                case "category": sortStr += "i.category"; break;
                case "closePrice": sortStr += "n.closePrice"; break;
                default: sortStr += "i.code"; break;
            }
            string strsql = string.Format(sql, sortStr, sortDESC, pageStr);

            List<StockFavoriteDTO> rs = _db.StockFavoriteDTO.FromSqlRaw(strsql,
                            new SqlParameter("@operId", f.operId),
                            new SqlParameter("@OFFSET", ((f.options.page - 1) * f.options.itemsPerPage)),
                            new SqlParameter("@FETCH", f.options.itemsPerPage)).ToList();
            if (rs.Count == 0)
            {
                return new List<StockFavoriteDTO>();
            }
            return rs;

        }

        public string Create(stockFavorite f)
        {
            int status = 0;
            var check = _db.stockIndex.Where(x => (x.code == f.code || x.company == f.code) && x.isEnable == true).FirstOrDefault();
            if(check != null)
            {
                var res = _db.stockFavorite.Where(x => x.operId == f.operId && x.code == check.code).FirstOrDefault();
                if(res == null)
                {
                    stockFavorite data = new stockFavorite();
                    data.operId = f.operId;
                    data.code = check.code;
                    _db.stockFavorite.Add(data);
                    status = _db.SaveChanges();
                    return status > 0 ? "01" : "02";
                }
                else
                {
                    //已存在無須新增
                    return "03";
                }
            }
            else
            {
                return "02";
            }
        }

        public bool Edit(stockFavorite f)
        {
            int status = 0;
            var destination = _db.stockFavorite.Where(x => x.id == f.id).FirstOrDefault();
            Mapper.Initialize(cfg => cfg.CreateMap<stockFavorite, stockFavorite>()
                                        .ForMember(x => x.id, opt => opt.Ignore())
                                        .ForMember(x => x.operId, opt => opt.Ignore())
                                        .ForMember(x => x.code, opt => opt.Ignore()));
            Mapper.Map(f, destination);
            status = _db.SaveChanges();
            return status > 0 ? true : false;
        }

        public bool Delete(int id)
        {
            int status = 0;
            var del = _db.stockFavorite.Where(x => x.id == id).FirstOrDefault();
            _db.stockFavorite.Remove(del);
            status = _db.SaveChanges();

            return status > 0 ? true : false;
        }

    }
}
