using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iCTR.TB.Framework.Filter;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreFrame.WebApi
{
    /// <summary>
    /// 异常处理类
    /// </summary>
    public class ExceptionProcess : IExceptionProcess
    {
        /// <summary>
        /// 异常处理方法
        /// </summary>
        /// <param name="exception"></param>
        public void Process(ExceptionContext exception)
        {
            //可以实现自己的异常处理逻辑
        }
    }
}
