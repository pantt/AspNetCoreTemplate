using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreFrame.Entities;
using NetCoreFrame.Entities.Repositories;
using NetCoreFrame.Repository.EF.Repositories;
using iCTR.TB.Framework.Core.Extensions;
namespace NetCoreFrame.Repository.EF
{
    /// <summary>
    /// 仓储和事务管理单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataContext"></param>
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionstring"></param>
        public UnitOfWork(string connectionstring)
        {
            _connectionString = connectionstring;
            DbContextOptionsBuilder options = new DbContextOptionsBuilder();

            var option = options.UseMySql(connectionstring).Options;
            _dataContext = new DataContext(option);
        }
        /// <summary>
        /// user仓储
        /// </summary>
        public IUserRepository UserRepository
        {
            get { return _userRepository = _userRepository ?? new UserRepository(_dataContext); }
        }

        /// <summary>
        /// sample仓储
        /// </summary>
        public ISampleRepository SampleRepository
        {
            get { return _sampleRepository = _sampleRepository ?? new SampleRepository(_dataContext); }
        }
       

        /// <summary>
        /// 执行Sql语句，返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="commandType">sql语句类型</param>
        /// <param name="args">sql参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteSql(string sql, CommandType commandType = CommandType.Text, params object[] args)
        {
            using (var command = _dataContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = commandType;
                command.Parameters.AddRange(args);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行Sql语句，返回结果集
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <returns>查询结果集</returns>
        public List<T> QueryListBySql<T>(string sql)
        {
            List<T> entities;
            using (var command = _dataContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (var result = command.ExecuteReader())
                {
                    entities = result.ReaderToList<T>().ToList();
                }
            }
            return entities;
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void Save()
        {
            _dataContext.SaveChanges();
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            _dataContext.Database.OpenConnection();
            _transaction = _dataContext.Database.GetDbConnection().BeginTransaction();
            _dataContext.Database.UseTransaction((DbTransaction)_transaction);
            return _transaction;
        }
        /// <summary>
        /// 事务提交
        /// </summary>
        public void Complete()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            if (_connection == null) return;
            _connection.Dispose();
            _connection.Close();
        }

        #region 私有成员
        /// <summary>
        /// 数据连接字符串
        /// </summary>
        private readonly string _connectionString;
        /// <summary>
        /// 数据连接
        /// </summary>
        private IDbConnection _connection;
        /// <summary>
        /// 事务
        /// </summary>
        private IDbTransaction _transaction;
        /// <summary>
        /// 数据上下文
        /// </summary>
        private readonly DataContext _dataContext;

        /// <summary>
        /// sample仓储
        /// </summary>
        private ISampleRepository _sampleRepository;

        /// <summary>
        /// user仓储
        /// </summary>
        private IUserRepository _userRepository;
        #endregion

    }
}