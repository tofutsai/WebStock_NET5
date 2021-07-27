using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebStock_NET5.DAL;
using static WebStock_NET5.Common;

namespace WebStock_NET5.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockDownloadController : ControllerBase
    {
        private IStockDownloadDAL _IStockDownloadDAL;
        private IStockSysConfigDAL _IStockSysConfigDAL;

        public StockDownloadController(IStockDownloadDAL stockDownloadDAL, IStockSysConfigDAL stockSysConfigDAL)
        {
            _IStockDownloadDAL = stockDownloadDAL;
            _IStockSysConfigDAL = stockSysConfigDAL;
        }

        [HttpPost]
        public async Task<JsonResult> DownloadStockData()
        {
            bool status = true;
            string msg = "";

            try
            {
                DateTime date = _IStockSysConfigDAL.GetsysConfigstockUpdate();
                int count = 0;
                while (date <= DateTime.UtcNow.AddHours(08))
                {
                    string datetime = date.ToString("yyyy-MM-dd");
                    string s = await _IStockDownloadDAL.DownloadStock(datetime);
                    msg += s + "<br/>";
                    DateTime nextdate = date.AddDays(1);
                    count = _IStockSysConfigDAL.EditsysConfigstockUpdate(nextdate);
                    date = nextdate;
                    Thread.Sleep(5000);
                }
                date = DateTime.UtcNow.AddHours(08);
                count = _IStockSysConfigDAL.EditsysConfigstockUpdate(date);
                status = true;
            }
            catch (Exception e)
            {
                status = false;
                msg += e.Message;
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
        public async Task<JsonResult> DownloadOtcData()
        {
            bool status = true;
            string msg = "";
            try
            {
                DateTime date = _IStockSysConfigDAL.GetsysConfigotcUpdate();
                int count = 0;
                while (date <= DateTime.UtcNow.AddHours(08))
                {
                    DateTime taiwandatetime = date.AddYears(-1911);
                    string datetime = taiwandatetime.ToString("yyyy/MM/dd");
                    string s = await _IStockDownloadDAL.DownloadOtc(datetime);
                    msg += s + "<br/>";
                    DateTime nextdate = date.AddDays(1);
                    count = _IStockSysConfigDAL.EditsysConfigotcUpdate(nextdate);
                    date = nextdate;
                    Thread.Sleep(5000);
                }
                date = DateTime.UtcNow.AddHours(08);
                count = _IStockSysConfigDAL.EditsysConfigotcUpdate(date);
                status = true;
            }
            catch (Exception e)
            {
                status = false;
                msg += e.Message;
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
