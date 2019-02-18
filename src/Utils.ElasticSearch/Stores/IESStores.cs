using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.ElasticSearch.Stores
{
    public interface IESStores<TEntity> where TEntity : class
    {
        /// <summary>
        /// 新增doc
        /// id==null,Automatic ID Generation
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        /// <param name="id"></param>
        /// <param name="typeName"></param>
        void Add(TEntity entity, string index, Nest.Id id=null,string typeName=null);

        void Add(TEntity entity, string index, Expression<Func<TEntity, object>> expression, string typeName = null);

        void Add(IEnumerable<TEntity> entities, string index, Expression<Func<TEntity, object>> expression, string typeName = null);

        /// <summary>
        /// 新增doc
        /// id==null,Automatic ID Generation
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="index"></param>
        /// <param name="id"></param>
        /// <param name="typeName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity, string index, Nest.Id id=null, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));

        Task AddAsync(TEntity entity, string index, Expression<Func<TEntity, object>> expression, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));

        Task AddAsync(IEnumerable<TEntity> entities, string index, Expression<Func<TEntity, object>> expression, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));

        void Update(TEntity entity, Nest.Id id, string index, string typeName = null);

        void Update(TEntity entity, Expression<Func<TEntity, object>> expression, string index, string typeName = null);
        void Update(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> expression, string index, string typeName = null);
        Task UpdateAsync(TEntity entity, Nest.Id id, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> expression, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> expression, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">标识</param>
        void Remove(object id, string index, string typeName = null);

        /// <summary>
        /// 移除实体集合
        /// </summary>
        /// <param name="ids">标识集合</param>
        void Remove(IEnumerable<object> ids, string index, string typeName = null);

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">标识</param>
        void Remove(Nest.Id id, string index, string typeName = null);

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task RemoveAsync(object id, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 移除实体集合
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task RemoveAsync(IEnumerable<object> ids, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">标识</param>
        Task RemoveAsync(Nest.Id id, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));

        Task RemoveAsync(IEnumerable<Nest.Id> ids, string index, string typeName = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
