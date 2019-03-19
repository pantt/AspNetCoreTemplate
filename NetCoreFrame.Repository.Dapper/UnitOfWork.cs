using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;
using Dapper;
using MySql.Data.MySqlClient;
using NetCoreFrame.Entities;
using NetCoreFrame.Entities.Repositories;
using NetCoreFrame.Repository.Dapper.Repositories;

namespace NetCoreFrame.Repository.Dapper
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public UnitOfWork(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        #endregion

        /// <summary>
        /// 用户仓储
        /// </summary>

        public IUserRepository UserRepository
            => _userRepository ?? (_userRepository = new UserRepository(_transaction, _connection));

        /// <summary>
        /// 示例仓储
        /// </summary>
        public ISampleRepository SampleRepository
            => _sampleRepository ?? (_sampleRepository = new SampleRepository(_transaction, _connection));

        /// <summary>
        /// 执行Sql语句，返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">sql语句类型</param>
        /// <param name="args">sql参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteSql(string sql, CommandType commandType = CommandType.Text, params object[] args)
        {
            return _connection.Execute(sql, transaction: _transaction, commandType: commandType, param: args);
        }
        /// <summary>
        /// 执行Sql语句，返回结果集
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <returns>查询结果集</returns>
        public List<T> QueryListBySql<T>(string sql)
        {
            return _connection.Query<T>(sql).ToList();
        }


        /// <summary>
        /// 事务提交
        /// </summary>
        public void Save()
        {
            try
            {
                _transaction?.Commit();
            }
            catch (Exception)
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }


        /// <summary>
        /// 手动开启事务
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        /// <summary>
        /// 回收，资源释放
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            if (_connection == null) return;
            _connection.Dispose();
            _connection.Close();
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 析构函数
        /// </summary>
        ~UnitOfWork() => Dispose();

        #region 私有属性

        /// <summary>
        /// 数据连接
        /// </summary>
        private readonly IDbConnection _connection;

        /// <summary>
        /// 事务
        /// </summary>
        private IDbTransaction _transaction;

        /// <summary>
        /// 示例仓储
        /// </summary>
        private ISampleRepository _sampleRepository;

        /// <summary>
        /// 用户仓储
        /// </summary>
        private IUserRepository _userRepository;

        #endregion
    }
}