using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreFrame.WebApi.Authorize
{
    /// <summary>
    /// Bear授权属性
    /// </summary>
    public class BearerAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BearerAuthorizeAttribute() : base("Bearer") { }
    }
}
