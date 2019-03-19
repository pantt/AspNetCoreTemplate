using System.Data;
using NetCoreFrame.Entities.Models;
using NetCoreFrame.Entities.Repositories;
using NetCoreFrame.Repository.Dapper.Base;

namespace NetCoreFrame.Repository.Dapper.Repositories
{
    /// <summary>
    /// 用户仓储实现
    /// </summary>
    public class UserRepository : RepositoryBase<User, string>, IUserRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="connection">数据库连接</param>
        public UserRepository(IDbTransaction transaction, IDbConnection connection) : base(transaction, connection)
        {
        }
    }
}