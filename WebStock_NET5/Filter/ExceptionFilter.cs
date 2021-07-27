using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebStock_NET5.Common;

namespace WebStock_NET5.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var e = filterContext.Exception;
            var inner = "";
            var inner2 = "";
            if (e.InnerException != null)
            {
                inner = e.InnerException.Message;

                if (e.InnerException.InnerException != null)
                    inner2 = e.InnerException.InnerException.Message;
            }
            filterContext.Result = new JsonResult(new Results<DBNull>
            {
                Data = null,
                Success = false,
                Message = string.Format("{0}\n{1}\n{2}", e.Message, inner, inner2),
                Code = "500"
            });
            filterContext.ExceptionHandled = true;
        }
    }
}
