using System;
using iCTR.TB.Framework.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCoreFrame.Common.Authentication;
using Newtonsoft.Json;

namespace NetCoreFrame.WebApi.Controllers
{
    /// <summary>
    /// 用户登录相关api接口
    /// </summary>
    [Produces("application/json")]
    [Authorize("Bearer")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        /// <summary>
        /// session管理
        /// </summary>
        private readonly IUserContext _userContext;

        /// <summary>
        /// 配置文件信息实体
        /// </summary>
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// 权限类
        /// </summary>
        private AuthFactory _authFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtOptions">配置文件信息实体</param>
        /// <param name="userContext">session管理</param>
        /// <param name="authFactory">配置文件信息实体</param>
        public TokenController(IOptions<JwtOptions> jwtOptions, IUserContext userContext,
            AuthFactory authFactory)
        {
            _userContext = userContext;
            _jwtOptions = jwtOptions.Value;
            _authFactory = authFactory;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto">登录用户信息</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]UserDto dto)
        {
            if (dto.UserName == "11" & dto.Pwd == "11")
            {
                double expiresMinutes = 600;
                var response = new
                {
                    Status = true,
                    access_token = _authFactory.SignIn(new UserInfo() { UserName = "aa", UserId = "11" }), //用户token
                    expires_in = (int)TimeSpan.FromMinutes(expiresMinutes).TotalSeconds, //token过期时长
                    token_type = "Bearer"
                };
                return Ok(response);
            }
            return BadRequest("用户账号或密码错误！");
        }

        /// <summary>
        /// 登录后get测试方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery]string str)
        {
            return Ok(_userContext.UserName + str + "___" + _jwtOptions.Issuer);
        }
    }
    /// <summary>
    /// userDto
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
    }
}