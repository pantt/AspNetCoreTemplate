using NetCoreFrame.Entities.Models;
using NetCoreFrame.Entities.Repositories;
using NetCoreFrame.Repository.EF.Base;

namespace NetCoreFrame.Repository.EF.Repositories
{
    /// <summary>
    /// sample仓储实现类
    /// </summary>
    public class UserRepository : RepositoryBase<User, string>, IUserRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataContext">数据访问上下文</param>
        public UserRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}