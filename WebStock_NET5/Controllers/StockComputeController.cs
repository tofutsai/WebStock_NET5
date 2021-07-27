using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStock_NET5.DAL;
using static WebStock_NET5.Common;

namespace WebStock_NET5.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockComputeController : ControllerBase
    {
        private IStockComputeDAL _IStockComputeDAL;
        public StockComputeController(IStockComputeDAL stockComputeDAL)
        {
            _IStockComputeDAL = stockComputeDAL;
        }

        [HttpPost]
        public JsonResult ComputeStockAvg()
        {
            bool status = true;
            string msg = "";

            status = _IStockComputeDAL.stockAvgStatistics();
            msg = status ? "計算成功!" : "計算失敗!";

            return new JsonResult(new Results<DBNull>
            {
                Success = status,
                Message = msg,
                Data = null,
                TotalCount = status ? 1 : 0
            });
        }

        [HttpPost]
        public JsonResult ComputeStockNow()
        {
            bool status = true;
            string msg = "";

            status = _IStockComputeDAL.stockNowsStatistics();
            msg = status ? "計算成功!" : "計算失敗!";

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
