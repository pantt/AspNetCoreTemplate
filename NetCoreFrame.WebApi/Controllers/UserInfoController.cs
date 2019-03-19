using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreFrame.Application.Users;
using NetCoreFrame.Application.Users.Dtos;
using NetCoreFrame.Entities;

namespace NetCoreFrame.WebApi.Controllers
{
    /// <summary>
    /// 用户信息api接口
    /// </summary>
    [Produces("application/json")]
    //[Authorize("Bearer")]
    [ApiVersion("1.0")]
    [Route("api/UserInfo")]
    public class UserInfoController : Controller
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// 用户服务类
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userService">用户服务类</param>
        public UserInfoController(IUnitOfWork unitOfWork,IUserService userService)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 根据用户Id获取用户信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns>返回指定用户信息</returns>
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]string id)
        {
            return Ok(_userService.GetUser(id));
        }

        /// <summary>
        /// 根据查询条件获取分页用户信息
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns>返回符合条件的用户信息数组</returns>
        [HttpGet]
        public IActionResult GetByPage([FromQuery]UserPageInDto dto)
        {
            return Ok(_userService.GetUsersByPage(dto));
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="dto">待添加用户信息</param>
        /// <returns>返回添加的用户信息</returns>
        [HttpPost]
        public IActionResult Post([FromBody] UserCreateDto dto)
        {
            var result=_userService.CreateUser(dto);

            return Ok();
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="dto">待更新的用户信息</param>
        /// <returns>返回更新后的用户信息</returns>
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]string id,[FromBody] UserCreateDto dto)
        {
            return Ok(_userService.UpdateUser(id,dto));
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户主键Id</param>
        /// <returns>返回空</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _userService.DeleteUser(id);
            return Ok();
        }
        /// <summary>
        /// 事务实例
        /// </summary>
        /// <returns></returns>
        [HttpPost("TransactionTest")]
        public IActionResult TransactionTest()
        {
            _userService.TransactionTest();
            return Ok();
        }
    }
}