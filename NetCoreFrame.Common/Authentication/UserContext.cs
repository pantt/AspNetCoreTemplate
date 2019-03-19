using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NetCoreFrame.Common.Authentication;

namespace NetCoreFrame.Common.Authentication
{
    /// <summary>
    /// 用户上下文管理
    /// </summary>
    public class UserContext : IUserContext
    {
        /// <summary>
        /// http上下文
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Claims信息
        /// </summary>
        private readonly ClaimsPrincipal _principal;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessor">http上下文</param>
        public UserContext(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
            //_principal = principal as ClaimsPrincipal;
            _principal = _httpContextAccessor.HttpContext.User as ClaimsPrincipal;
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return _principal?.FindFirst(CtrClaimTypes.UserName).Value; }
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            get { return _principal?.FindFirst(CtrClaimTypes.RoleName).Value; }
        }
    }
}