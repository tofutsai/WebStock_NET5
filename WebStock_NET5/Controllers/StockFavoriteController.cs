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
    public class StockFavoriteController : ControllerBase
    {
        private IStockFavoriteDAL _IStockFavoriteDAL;
        public StockFavoriteController(IStockFavoriteDAL stockFavoriteDAL)
        {
            _IStockFavoriteDAL = stockFavoriteDAL;
        }
        [HttpPost]
        public JsonResult Read(FormSearch f)
        {
            bool status = true;
            bool check = true;
            List<StockFavoriteDTO> data = null;
            string msg = "";
            if (f.operId == 0)
            { check = false; }

            if (check)
            {
                data = _IStockFavoriteDAL.Read(f);
                status = data != null ? true : false;
                msg = status ? "查詢成功!" : "查詢失敗!";
            }
            else
            {
                status = false;
                msg = "資料輸入錯誤!";
            }

            return new JsonResult(new Results<List<StockFavoriteDTO>>
            {
                Success = status,
                Message = msg,
                Data = data,
                TotalCount = status ? data.Count() : 0
            });
        }

        [HttpPost]
        public JsonResult Create(stockFavorite f)
        {
            bool status = true;
            bool check = true;
            string msg = "";

            if (f.operId == 0)
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.code))
            {
                check = false;
            }
            if (check)
            {
                string s = _IStockFavoriteDAL.Create(f);
                status = s != "02" ? true : false;
                msg = s != "02" ? s != "03" ? "新增成功!" : "無須新增!" : "新增失敗!";
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
        public JsonResult Edit(stockFavorite f)
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
                status = _IStockFavoriteDAL.Edit(f);
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

        [HttpPost]
        public JsonResult Delete(stockFavorite f)
        {
            bool status = true;
            bool check = true;
            string msg = "";

            if (f.id == 0)
            {
                check = false;
            }
            if (f.operId == 0)
            {
                check = false;
            }
            if (check)
            {
                status = _IStockFavoriteDAL.Delete(f.id);
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
