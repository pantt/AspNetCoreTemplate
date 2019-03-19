using System;
using System.Data;
using NetCoreFrame.Entities.Models;
using NetCoreFrame.Entities.Repositories;
using NetCoreFrame.Repository.Dapper.Base;

namespace NetCoreFrame.Repository.Dapper.Repositories
{
    /// <summary>
    /// 示例仓储实体
    /// </summary>
    public class SampleRepository : RepositoryBase<Sample, Guid>, ISampleRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="connection">数据库连接</param>
        public SampleRepository(IDbTransaction transaction,IDbConnection connection) : base(transaction,connection)
        {
        }
    }
}