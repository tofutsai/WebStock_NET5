using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebStock_NET5.Common;

namespace WebStock_NET5.Filter
{
    public class ActionFilter : IActionFilter
    {
        public UserInfo UserInfo { get; set; }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //獲取action名稱
            var actionName = (string)filterContext.RouteData.Values["action"];
            var header = filterContext.HttpContext.Request.Headers["Content-Language"];
            if(actionName != "Login")
            {
                if (!string.IsNullOrEmpty(header))
                {
                    UserInfo = DecodeJWTToken(header);
                    if(UserInfo == null)
                    {
                        throw new Exception("Authorization Access denied");
                    }
                }
                else
                {
                    throw new Exception("Authorization Access denied");
                }
            }
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //context.HttpContext.Response.WriteAsync($"{GetType().Name} out. \r\n");
        }
    }
}
