using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStock_NET5.DAL;
using WebStock_NET5.DTO;
using static WebStock_NET5.Common;

namespace WebStock_NET5.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockStatisticsController : ControllerBase
    {
        private IStockStatisticsDAL _StockStatisticsDAL;
        public StockStatisticsController(IStockStatisticsDAL stockStatisticsDAL)
        {
            _StockStatisticsDAL = stockStatisticsDAL;
        }
        [HttpPost]
        public JsonResult Read(FormSearch f)
        {
            bool status = true;
            bool check = true;
            List<StockStatisticsDTO> data = null;
            string msg = "";

            //FormSearch f = new FormSearch();
            //f.options.page = 1;
            //f.options.itemsPerPage = 10;
            //f.options.sortBy = new string[] { "id" };
            //f.options.sortDesc = new bool[] { false };
            //f.type = "上";

            if (string.IsNullOrEmpty(f.type))
            { check = false; }

            if (check)
            {
                data = _StockStatisticsDAL.Read(f);
                status = data.Count() > 0 ? true : false;
                msg = status ? "查詢成功!" : "查詢失敗!";
            }
            else
            {
                status = false;
                msg = "資料輸入錯誤!";
            }

            return new JsonResult(new Results<List<StockStatisticsDTO>>
            {
                Success = status,
                Message = msg,
                Data = data,
                TotalCount = status ? data.FirstOrDefault().totalCount : 0
            });
        }
    }
}
