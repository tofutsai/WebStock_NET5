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
    public class StockSysLogController : ControllerBase
    {
        private IStockSysLogDAL _stockSysLogDAL;

        public StockSysLogController(IStockSysLogDAL stockSysLogDAL)
        {
            _stockSysLogDAL = stockSysLogDAL;
        }
        [HttpPost]
        public JsonResult Read(FormSearch f)
        {
            bool status = true;
            bool check = true;
            List<StockSysLogDTO> data = null;
            string msg = "";
            //FormSearch f = new FormSearch();
            //f.code = "2330";
            //f.dataDate = DateTime.UtcNow.AddMonths(-2);
            //f.options.page = 1;
            //f.options.itemsPerPage = 10;
            //f.options.sortBy = new string[] { "id" };
            //f.options.sortDesc = new bool[] { false };
            //f.type = "上";
            if (f.dataDate == DateTime.MinValue)
            {
                check = false;
            }
            if (check)
            {
                data = _stockSysLogDAL.Read(f);
                status = data.Count() > 0 ? true : false;
                msg = status ? "查詢成功!" : "查詢失敗!";
            }

            return new JsonResult(new Results<List<StockSysLogDTO>>
            {
                Success = status,
                Message = msg,
                Data = data,
                TotalCount = status ? data.FirstOrDefault().totalCount : 0
            });
        }
    }
}
