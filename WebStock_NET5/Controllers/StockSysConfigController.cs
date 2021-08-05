using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStock_NET5.DAL;
using WebStock_NET5.DB;
using WebStock_NET5.DTO;
using static WebStock_NET5.Common;

namespace WebStock_NET5.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockSysConfigController : ControllerBase
    {
        private IStockSysConfigDAL _StockSysConfigDAL;

        public StockSysConfigController(IStockSysConfigDAL StockSysConfigDAL)
        {
            _StockSysConfigDAL = StockSysConfigDAL;
        }

        [HttpPost]
        public JsonResult Read()
        {
            bool status = true;
            List<StockSysConfigDTO> data = null;
            string msg = "";

            data = _StockSysConfigDAL.Read();
            status = data.Count() > 0 ? true : false;
            msg = status ? "查詢成功!" : "查詢失敗!";

            return new JsonResult(new Results<List<StockSysConfigDTO>>
            {
                Success = status,
                Message = msg,
                Data = data,
                TotalCount = status ? data.FirstOrDefault().totalCount : 0
            });
        }

        [HttpPost]
        public JsonResult Edit(sysConfig f)
        {
            bool status = true;
            bool check = true;
            string msg = "";
            if(f.stockUpdate == DateTime.MinValue || f.otcUpdate == DateTime.MinValue || f.avgStartDate == DateTime.MinValue
                || f.avgEndDate == DateTime.MinValue || f.nowDate == DateTime.MinValue)
            {
                check = false;
            }
            if (check)
            {
                status = _StockSysConfigDAL.Edit(f);
                msg = status ? "更新成功!" : "更新失敗!";
            }
            else
            {
                status = false;
                msg = "資料輸入錯誤!";
            }

            return new JsonResult(new Results<DBNull>
            {
                Success = status,
                Message = msg,
                Data = null,
                TotalCount = status ? 1 : 0
            });
        }
    }
}
