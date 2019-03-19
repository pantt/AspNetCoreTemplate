using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using iCTR.TB.Framework.Core.Extensions;
using iCTR.TB.Framework.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NetCoreFrame.Entities.Repositories;

namespace NetCoreFrame.Repository.EF.Base
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        //定义数据访问上下文对象
        protected readonly DataContext _dbContext;

        protected DbSet<TEntity> _dbSet;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryBase(DataContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();

            
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAllList()
        {
            return _dbSet.ToList();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity Get(TPrimaryKey id)
        {
            return _dbSet.FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 查询某个条件的值是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体集合</param>
        public void InsertAll(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public TEntity Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">实体集合</param>
        public void UpdateAll(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public TEntity InsertOrUpdate(TEntity entity, TPrimaryKey id)
        {
            if (Get(id) != null)
                return Update(entity);
            return Insert(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        public void Delete(TPrimaryKey id)
        {
            _dbSet.Remove(Get(id));
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体集合</param>
        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize,
            string sortField,
            SortBy sortBy = SortBy.Ascending)
        {
            IQueryable<TEntity> query = null;
            if (predicate == null)
            {
                query = _dbSet.Where(w => 1 == 1);
            }
            else
            {
                query = _dbSet.Where(predicate);
            }
            if (sortBy == SortBy.Ascending)
                return query.OrderBy(sortField).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    .ToList();
            return query.OrderByDescending(sortField).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ToList();
        }
        /// <summary>
        /// 部分更新方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Update(TPrimaryKey id, object entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}