using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStock_NET5.DAL;
using WebStock_NET5.DB;
using static WebStock_NET5.Common;

namespace WebStock_NET5.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockMemberController : ControllerBase
    {
        private IStockMemberDAL _IStockMemberDAL;
        public StockMemberController(IStockMemberDAL stockMemberDAL)
        {
            _IStockMemberDAL = stockMemberDAL;
        }

        [HttpPost]
        public JsonResult Login(member f)
        {
            bool status = true;
            bool check = true;
            UserInfo data = null;
            string msg = "";
            if (string.IsNullOrEmpty(f.account))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.password))
            {
                check = false;
            }
            if (check)
            {
                member member = _IStockMemberDAL.Login(f);
                if (member != null)
                {
                    var payload = new UserInfo
                    {
                        OperId = member.id,
                        OperAccount = member.account,
                        OperName = member.name,
                        OperIsAdmin = member.isAdmin
                    };
                    string JWTToken = EncodeJWTToken(payload);
                    data = new UserInfo();
                    data.OperId = member.id;
                    data.OperAccount = member.account;
                    data.OperName = member.name;
                    data.OperIsAdmin = member.isAdmin;
                    data.JWToken = JWTToken;
                }

                status = data != null ? true : false;
                msg = status ? "登入成功!" : "登入失敗!";   
            }
            else
            {
                status = false;
                msg = "資料輸入錯誤!";
            }
            return new JsonResult(new Results<UserInfo>
            {
                Success = status,
                Message = msg,
                Data = data,
                TotalCount = status ? 1 : 0
            });
        }

        [HttpPost]
        public JsonResult Register(member f)
        {
            bool status = true;
            bool check = true;
            string msg = "";
            if (string.IsNullOrEmpty(f.account))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.password))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(f.name))
            {
                check = false;
            }
            if (check)
            {
                string s = _IStockMemberDAL.Register(f);
                status = s != "02" ? true : false;
                msg = s != "02" ? s != "03" ? "註冊成功!" : "此帳號已有人註冊!" : "註冊失敗!";
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
        public JsonResult EditPassword(EditPas editPas)
        {
            bool status = true;
            bool check = true;
            string msg = "";
            if (string.IsNullOrEmpty(editPas.oldPassword))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(editPas.newPassword))
            {
                check = false;
            }
            if (string.IsNullOrEmpty(editPas.newPasswordCheck))
            {
                check = false;
            }
            if (editPas.newPassword != editPas.newPasswordCheck)
            {
                check = false;
                status = false;
                msg = "新密碼不一致，請重新輸入";
            }
            if (check)
            {
                string s = _IStockMemberDAL.EditPassword(editPas);
                status = s != "02" && s != "03" ? true : false;
                msg = s != "02" ? s != "03" ? "密碼修改成功!" : "舊密碼輸入錯誤!" : "密碼修改失敗!";
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
