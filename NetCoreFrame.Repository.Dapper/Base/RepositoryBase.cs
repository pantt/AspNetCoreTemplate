using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using iCTR.TB.Dapper.Extensions.Query;
using iCTR.TB.Framework.Domain.Entities;
using NetCoreFrame.Entities.Repositories;

namespace NetCoreFrame.Repository.Dapper.Base
{
    /// <summary>
    /// 仓储实现抽象类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TPrimaryKey">主键</typeparam>
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="connection"></param>
        protected RepositoryBase(IDbTransaction transaction, IDbConnection connection)
        {
            Transaction = transaction as DbTransaction;
            Connection = connection as DbConnection;
            DapperExtensions.DapperExtensions.SqlDialect = new MySqlDialect();
            this.DefaultSort = "ASC";
            DapperConfig =
                new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new MySqlDialect());
            SqlGenerator = new SqlGeneratorImpl(DapperConfig);
        }

        /// <summary>
        /// 默认排序方式
        /// </summary>
        public string DefaultSort { get; set; }

        /// <summary>
        /// Dapper配置
        /// </summary>
        private DapperExtensionsConfiguration DapperConfig { get; set; }

        /// <summary>
        /// Sql构造器
        /// </summary>
        private ISqlGenerator SqlGenerator { get; set; }

        /// <summary>
        /// 事务
        /// </summary>
        protected DbTransaction Transaction { get; private set; }

        /// <summary>
        /// 数据连接
        /// </summary>
        protected DbConnection Connection { get; set; }

        /// <summary>
        /// Dapper查询转换辅助类
        /// </summary>
        private DapperQueryFilterExecuter dapperQueryFilterExecuter => new DapperQueryFilterExecuter();

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public List<TEntity> GetAllList()
        {
            return Connection.GetList<TEntity>().ToList();
        }

        /// <summary>
        /// 返回IQueryable以便条件查询
        /// </summary>
        /// <returns>IQueryable实体列表</returns>
        public IQueryable<TEntity> GetAll()
        {
            return Connection.GetList<TEntity>().AsQueryable();
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns>实体列表</returns>
        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            var pd = dapperQueryFilterExecuter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
            return Connection.GetList<TEntity>(pd).ToList();
        }

        /// <summary>
        /// 获取表数据总数
        /// </summary>
        /// <returns>数据行数</returns>
        public int Count()
        {
            return Connection.Count<TEntity>(null);
        }

        /// <summary>
        /// 获取符合lambda表达式的数据条数
        /// </summary>
        /// <param name="predicate">lambda表达式</param>
        /// <returns>数据行数</returns>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            var pd = dapperQueryFilterExecuter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
            return Connection.Count<TEntity>(pd);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns>实体</returns>
        public TEntity Get(TPrimaryKey id)
        {
            return Connection.Get<TEntity>((TPrimaryKey) id);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns>实体</returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var pd = dapperQueryFilterExecuter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
            return Connection.GetList<TEntity>(pd).FirstOrDefault();
        }

        /// <summary>
        /// 判断符合lambda表达的数据是否存在
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns>是否存在</returns>
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            var pd = dapperQueryFilterExecuter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
            return Connection.GetList<TEntity>(pd).Any();
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public TEntity Insert(TEntity entity)
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            List<IPropertyMap> nonIdentityKeyProperties = classMap.Properties
                .Where(p => p.KeyType == KeyType.Guid || p.KeyType == KeyType.Assigned).ToList();
            var identityColumn = classMap.Properties.SingleOrDefault(p => p.KeyType == KeyType.Identity);
            foreach (var column in nonIdentityKeyProperties)
                if (column.KeyType == KeyType.Guid && (Guid) column.PropertyInfo.GetValue(entity, null) == Guid.Empty)
                {
                    Guid comb = SqlGenerator.Configuration.GetNextGuid();
                    column.PropertyInfo.SetValue(entity, comb, null);
                }
            IDictionary<string, object> keyValues = new ExpandoObject();
            string sql = SqlGenerator.Insert(classMap);
            if (identityColumn != null)
            {
                IEnumerable<long> result;
                if (SqlGenerator.SupportsMultipleStatements())
                {
                    sql += SqlGenerator.Configuration.Dialect.BatchSeperator + SqlGenerator.IdentitySql(classMap);
                    result = Connection.Query<long>(sql, entity, Transaction, false);
                }
                else
                {
                    Connection.Execute(sql, entity, Transaction);
                    sql = SqlGenerator.IdentitySql(classMap);
                    result = Connection.Query<long>(sql, entity, Transaction, false);
                }
                long identityValue = result.First();
                int identityInt = Convert.ToInt32(identityValue);
                keyValues.Add(identityColumn.Name, identityInt);
                identityColumn.PropertyInfo.SetValue(entity, identityInt, null);
            }
            else
            {
                Connection.Execute(sql, entity, Transaction);
            }
            return entity;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体列表</param>
        public void InsertAll(IEnumerable<TEntity> entities)
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            var notKeyProperties = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            foreach (var e in entities)
            foreach (var column in notKeyProperties)
                if (column.KeyType == KeyType.Guid && (Guid) column.PropertyInfo.GetValue(e, null) == Guid.Empty)
                {
                    Guid comb = SqlGenerator.Configuration.GetNextGuid();
                    column.PropertyInfo.SetValue(e, comb, null);
                }
            string sql = SqlGenerator.Insert(classMap);
            Connection.Execute(sql, entities, Transaction);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public TEntity Update(TEntity entity)
        {
            var isUpdate = false;
            isUpdate = Connection.Update(entity, Transaction);
            if (isUpdate)
                return entity;
            else
                throw new Exception("数据更新失败");
        }

        /// <summary>
        /// 部分更新
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        public TEntity Update(TPrimaryKey id, object entity)
        {
            IClassMapper classMapper = DapperConfig.GetMap<TEntity>();
            IClassMapper entityMapper = DapperConfig.GetMap(entity.GetType());
            var keyName =
                classMapper.Properties.First(d => d.PropertyInfo.PropertyType == typeof(TPrimaryKey) &&
                                                  d.KeyType != KeyType.NotAKey);
            var predicate = new FieldPredicate<TEntity>
            {
                PropertyName = keyName.Name,
                Operator = Operator.Eq,
                Value = id,
                Not = false
            };
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            DynamicParameters dynamicParameters = new DynamicParameters();
            string sql = GenerateUpdateSql(classMapper, entityMapper, predicate, parameters);
            var columns =
                entityMapper.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            foreach (var property in ReflectionHelper.GetObjectValues(entity)
                .Where(property => columns.Any(c => c.Name == property.Key)))
                dynamicParameters.Add(property.Key, property.Value);
            foreach (var parameter in parameters)
                dynamicParameters.Add(parameter.Key, parameter.Value);
            Connection.Execute(sql, dynamicParameters, Transaction);
            return Get(id);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">实例列表</param>
        public void UpdateAll(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Update(entity);
        }

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        public TEntity InsertOrUpdate(TEntity entity, TPrimaryKey id)
        {
            var existEntity = Connection.Get<TEntity>(id);
            if (existEntity == null)
                return Insert(entity);
            else
                return Update(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        public void Delete(TEntity entity)
        {
            Connection.Delete(entity, Transaction);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        public void Delete(TPrimaryKey id)
        {
            IClassMapper classMapper = SqlGenerator.Configuration.GetMap<TEntity>();
            var key = classMapper.Properties.Single(d => d.Name.ToLower().Equals("id"));
            var table = DapperConfig.Dialect.GetTableName(classMapper.SchemaName, classMapper.TableName, null);
            var sql =
                new StringBuilder(string.Format("DELETE FROM {0} Where {1} = @id", table, key.ColumnName));
            var p = new DynamicParameters();
            p.Add("@id", id, DbType.Guid);
            Connection.Execute(sql.ToString(), p, Transaction);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体列表</param>
        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Connection.Delete(entity, Transaction);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortBy">是否升序</param>
        /// <returns>实体列表</returns>
        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize,
            string sortField,
            SortBy sortBy = SortBy.Ascending)
        {
            var pd = dapperQueryFilterExecuter.ExecuteFilter<TEntity, TPrimaryKey>(predicate);
            return Connection.GetPage<TEntity>(pd,
                new List<ISort> {new Sort {Ascending = sortBy == SortBy.Ascending, PropertyName = sortField}},
                pageIndex - 1, pageSize);
        }


        #region 辅助方法

        /// <summary>
        /// 根据对象的字段生成更新语句
        /// </summary>
        /// <param name="mainClassMap">类映射</param>
        /// <param name="updateClassMapper">实体映射</param>
        /// <param name="predicate">条件</param>
        /// <param name="parameters">参数</param>
        /// <returns>生成的Sql语句</returns>
        private string GenerateUpdateSql(IClassMapper mainClassMap, IClassMapper updateClassMapper,
            IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (predicate == null)
                throw new ArgumentNullException($"Predicate");
            if (parameters == null)
                throw new ArgumentNullException($"Parameters");
            var updateProperties = updateClassMapper.Properties.Select(d => d.Name);
            var columns = mainClassMap.Properties.Where(
                    p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity) &&
                         updateProperties.Contains(p.Name))
                .ToList();
            if (!columns.Any())
                throw new ArgumentException("No columns were mapped.");
            var setSql =
                columns.Select(
                    p =>
                        string.Format(
                            "{0} = {1}{2}", SqlGenerator.GetColumnName(mainClassMap, p, false),
                            DapperConfig.Dialect.ParameterPrefix, p.Name));
            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                SqlGenerator.GetTableName(mainClassMap),
                setSql.AppendStrings(),
                predicate.GetSql(SqlGenerator, parameters));
        }

        #endregion
    }
}