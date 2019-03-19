using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;
using NetCoreFrame.Entities.Repositories;

namespace NetCoreFrame.Entities
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// 示例仓储
        /// </summary>
        ISampleRepository SampleRepository { get; }
        /// <summary>
        /// 执行Sql语句，返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">sql语句类型</param>
        /// <param name="args">sql参数</param>
        /// <returns>影响行数</returns>
        int ExecuteSql(string sql, CommandType commandType = CommandType.Text, params object[] args);
        /// <summary>
        /// 执行Sql语句，返回结果集
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <returns>查询结果集</returns>
        List<T> QueryListBySql<T>(string sql);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //IDbTransaction BeginTransaction();
        /// <summary>
        /// 事务保存
        /// </summary>
        void Save();
        IDbTransaction BeginTransaction();
        void Complete();
    }
}