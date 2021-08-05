using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebStock_NET5.DB;
using WebStock_NET5.DTO;
using System.Collections.Generic;
using System.Linq;
using static WebStock_NET5.Common;

namespace WebStock_NET5.DAL
{
    public interface IStockIndexDAL
    {
        List<StockIndexDTO> Read(FormSearch f);
        bool Create(stockIndex f);
        bool Edit(stockIndex f);
        bool Delete(int id);

    }

    public class StockIndexDAL : IStockIndexDAL
    {
        private readonly WebStockContextDTO _db;

        public StockIndexDAL(WebStockContextDTO WebStockContextDTO)
        {
            _db = WebStockContextDTO;
        }

        public List<StockIndexDTO> Read(FormSearch f)
        {

            string sqlStr = @"
                            SELECT
                            count(1) over() as totalCount,
                            a.id,			
                            a.type,		
                            a.category,	
                            a.code,		
                            a.company,	
                            a.dataDate,
                            a.isEnable
                            FROM stockIndex a
                            WHERE
                            {0} 
                            {1} {2} 
                            {3}
                            
                            ";

            string whereStr = "";
            if (string.IsNullOrEmpty(f.code))
            {
                whereStr = @"
                            a.type LIKE '%'+ @type +'%'
                            ";
            }
            else
            {
                whereStr = @"
                            a.code LIKE @code
                            OR
                            a.company LIKE @code
                            ";
            }
            string sortStr = "ORDER BY ";
            string sortDESC = (f.options.sortDesc != null && f.options.sortDesc.Count() > 0 && f.options.sortDesc[0]) ? "DESC" : "ASC";
            string pageStr = (f.options.page != 0 && f.options.itemsPerPage != 0) ? "OFFSET @OFFSET ROWS FETCH NEXT @FETCH ROWS ONLY" : "";

            string sortByType = (f.options.sortBy != null && f.options.sortBy.Count() > 0 && f.options.sortBy[0] != "") ? f.options.sortBy[0] : "";
            switch (sortByType)
            {
                case "id": sortStr += "a.id"; break;
                case "type": sortStr += "a.type"; break;
                case "category": sortStr += "a.category"; break;
                case "code": sortStr += "a.code"; break;
                case "company": sortStr += "a.company"; break;
                case "dataDate": sortStr += "a.dataDate"; break;
                case "isEnable": sortStr += "a.isEnable"; break;
                default: sortStr += "a.id"; break;
            }



            sqlStr = string.Format(sqlStr, whereStr, sortStr, sortDESC, pageStr);
            List<StockIndexDTO> rs = _db.StockIndexDTO.FromSqlRaw(sqlStr,
                new SqlParameter("@type", f.type),
                new SqlParameter("@code", f.code ?? string.Empty),
                new SqlParameter("@OFFSET", ((f.options.page - 1) * f.options.itemsPerPage)),
                new SqlParameter("@FETCH", f.options.itemsPerPage)
                ).ToList();


            return rs;


        }

        public bool Create(stockIndex f)
        {
            int status = 0;

            _db.stockIndex.Add(f);
            status = _db.SaveChanges();
            return status > 0 ? true : false;
        }

        public bool Edit(stockIndex f)
        {
            int status = 0;

            var stockIndex = _db.stockIndex.Where(x => x.id == f.id).FirstOrDefault();
            Mapper.Initialize(cfg => cfg.CreateMap<stockIndex, stockIndex>()
                                              .ForMember(x => x.id, opt => opt.Ignore())
                                              .ForMember(x => x.code, opt => opt.Ignore())
                                              );
            Mapper.Map(f, stockIndex);
            status = _db.SaveChanges();
            return status >= 0 ? true : false;
        }

        public bool Delete(int id)
        {
            int status = 0;

            var stockIndex = _db.stockIndex.Where(x => x.id == id).FirstOrDefault();
            _db.stockIndex.Remove(stockIndex);
            status = _db.SaveChanges();
            return status > 0 ? true : false;
        }

    }
}
