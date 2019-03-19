using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using iCTR.TB.Framework.Domain.Entities;

namespace NetCoreFrame.Entities.Repositories
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// 返回IQueryable以便条件查询
        /// </summary>
        /// <returns>IQueryable实体列表</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns>实体列表</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取表数据总数
        /// </summary>
        /// <returns>数据行数</returns>
        int Count();

        /// <summary>
        /// 获取符合lambda表达式的数据条数
        /// </summary>
        /// <param name="predicate">lambda表达式</param>
        /// <returns>数据行数</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns>实体</returns>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns>实体</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 判断符合lambda表达的数据是否存在
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns>是否存在</returns>
        bool Any(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体列表</param>
        void InsertAll(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// 部分更新
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="entity">实体</param>
        /// <returns>实体</returns>
        TEntity Update(TPrimaryKey id, object entity);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">实例列表</param>
        void UpdateAll(IEnumerable<TEntity> entities);

        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="id">主键</param>
        /// <returns>实体</returns>
        TEntity InsertOrUpdate(TEntity entity, TPrimaryKey id);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        void Delete(TPrimaryKey id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体列表</param>
        void DeleteAll(IEnumerable<TEntity> entities);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortBy">是否升序</param>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> GetPaged(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize,
            string sortField, SortBy sortBy = SortBy.Ascending);
    }
}