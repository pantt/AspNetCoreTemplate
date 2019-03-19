using NetCoreFrame.Entities.Models;

namespace NetCoreFrame.Entities.Repositories
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRepository : IRepository<User, string>
    {
    }
}