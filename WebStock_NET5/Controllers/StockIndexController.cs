using Microsoft.AspNetCore.Mvc;
using WebStock_NET5.DB;
using WebStock_NET5.DAL;
using WebStock_NET5.DTO;
using System.Collections.Generic;
using System.Linq;
using System;
using static WebStock_NET5.Common;

namespace NetCore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockIndexController : ControllerBase
    {
        private IStockIndexDAL _StockIndexDAL;

        public StockIndexController(IStockIndexDAL StockIndexDAL)
        {
            _StockIndexDAL = StockIndexDAL;
        }

        public class PostData
        {
            public int id { get; set; }
            public string type { get; set; }
            public string category { get; set; }
            public string code { get; set; }
            public string company { get; set; }
            public DateTime? dataDate { get; set; }
            public bool? isEnable { get; set; }
        }

        [HttpPost]
        public JsonResult Read(FormSearch f)
        {
            bool status = true;
            bool check = true;
            List<StockIndexDTO> data = null;
            string msg = "";

            //FormSearch f = new FormSearch();
            //f.options.page = 1;
            //f.options.itemsPerPage = 10;
            //f.options.sortBy = new string[] { "id" };
            //f.options.sortDesc = new bool[] { false };
            //f.type = "上";


            if (string.IsNullOrEmpty(f.type))
            {
                check = false;
            }

            if (check)
            {
                data = _StockIndexDAL.Read(f);
                status = data.Count() > 0 ? true : false;
                msg = status ? "查詢成功!" : "查詢失敗!";
            }
            else
            {
                status = false;
                msg = "資料輸入錯誤!";
            }

            return new JsonResult(new Results<List<StockIndexDTO>>
            {
                Success = status,
                Message = msg,
                Data = data,
                TotalCount = status ? data.FirstOrDefault().totalCount : 0
            });
        }

        [HttpPost]
        public JsonResult Create(stockIndex f)
        {
            bool status = true;
            bool check = true;
            string msg = "";

            if (string.IsNullOrEmpty(f.type))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.category))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.code))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.category))
            {
                check = false;
            }
            if (f.dataDate == DateTime.MinValue)
            {
                check = false;
            }
            if (f.isEnable == null)
            {
                check = false;
            }

            if (check)
            {
                stockIndex stockIndex = new stockIndex();
                stockIndex.type = f.type;
                stockIndex.category = f.category;
                stockIndex.code = f.code;
                stockIndex.company = f.company;
                stockIndex.dataDate = f.dataDate;
                stockIndex.isEnable = f.isEnable;

                status = _StockIndexDAL.Create(stockIndex);
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
        public JsonResult Edit(stockIndex f)
        {
            bool status = true;
            bool check = true;
            string msg = "";

            if (f.id == 0)
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.type))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.category))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.code))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.category))
            {
                check = false;
            }
            if (f.dataDate == DateTime.MinValue)
            {
                check = false;
            }
            if (f.isEnable == null)
            {
                check = false;
            }

            if (check)
            {
                status = _StockIndexDAL.Edit(f);
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
        public JsonResult Delete(PostData f)
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
                status = _StockIndexDAL.Delete(f.id);
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
