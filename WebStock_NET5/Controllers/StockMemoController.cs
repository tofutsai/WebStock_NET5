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
    public class StockMemoController : ControllerBase
    {
        private IStockMemoDAL _IStockMemoDAL;
        public StockMemoController(IStockMemoDAL stockMemoDAL)
        {
            _IStockMemoDAL = stockMemoDAL;
        }

        [HttpPost]
        public JsonResult Create(EditMemo editMemo)
        {
            bool status = true;
            bool check = true;
            string msg = "";

            if (string.IsNullOrEmpty(editMemo.type))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(editMemo.codes))
            {
                check = false;
            }

            if (check)
            {
                status = _IStockMemoDAL.Create(editMemo.type, editMemo.codes, editMemo.memoContent);
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
