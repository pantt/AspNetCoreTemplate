using iCTR.TB.Framework.Application;
using iCTR.TB.Framework.Domain.Entities;
using NetCoreFrame.Application.Users.Dtos;

namespace NetCoreFrame.Application.Users
{
    public interface IUserService : IService
    {
        /// <summary>
        /// 根据主键值获取一条用户数据
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>实体</returns>
        UserOutDto GetUser(string id);

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="dto">分页查询实体</param>
        /// <returns>分页用户数据列表</returns>
        PageOutDtoBase<UserOutDto> GetUsersByPage(UserPageInDto dto);

        /// <summary>
        /// 更新一条用户数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        UserOutDto UpdateUser(string id,UserCreateDto dto);

        /// <summary>
        /// 新增一条用户数据
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        UserOutDto CreateUser(UserCreateDto dto);

        /// <summary>
        /// 删除一条示例数据
        /// </summary>
        /// <param name="id">主键值</param>
        void DeleteUser(string id);
        void TransactionTest();
    }
}