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
    public class StockProfitController : ControllerBase
    {
        private IStockProfitDAL _IStockProfitDAL;
        public StockProfitController(IStockProfitDAL stockProfitDAL)
        {
            _IStockProfitDAL = stockProfitDAL;
        }

        public JsonResult Read(FormSearch f)
        {
            bool status = true;
            bool check = true;
            List<StockProfitDTO> data = null;
            string msg = "";

            if (f.operId == 0)
            { check = false; }

            if (check)
            {
                data = _IStockProfitDAL.Read(f);
                status = data != null ? true : false;
                msg = status ? "查詢成功!" : "查詢失敗!";
            }
            else
            {
                status = false;
                msg = "資料輸入錯誤!";
            }

            return new JsonResult(new Results<List<StockProfitDTO>>
            {
                Success = status,
                Message = msg,
                Data = data,
                TotalCount = status ? data.Count() : 0
            });
        }

        [HttpPost]
        public JsonResult Create(stockProfit f)
        {
            bool status = true;
            bool check = true;
            string msg = "";

            if(f.operId == 0 || string.IsNullOrEmpty(f.code) || f.buyPrice == 0 || f.buyShares == 0)
            {
                check = false;
            }
            if (check)
            {
                status = _IStockProfitDAL.Create(f);
                msg = status ? "新增成功!" : "新增失敗!";
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

        [HttpPost]
        public JsonResult Delete(stockProfit f)
        {
            bool status = true;
            bool check = true;
            string msg = "";

            if (f.id == 0)
            {
                check = false;
            }

            if (check)
            {
                status = _IStockProfitDAL.Delete(f.id);
                msg = status ? "刪除成功!" : "刪除失敗!";
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
