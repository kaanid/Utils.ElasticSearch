using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface IFieldClause
    {
        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="tableAlias">表别名</param>
        void Field(string columns, string tableAlias = null);
        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="propertyAsAlias">是否将属性名映射为列别名</param>
        void Field<TEntity>(Expression<Func<TEntity, object[]>> expression, bool propertyAsAlias = false) where TEntity : class;
        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="columnAlias">列别名</param>
        void Field<TEntity>(Expression<Func<TEntity, object>> expression, string columnAlias = null) where TEntity : class;

        IEnumerable<string> GetColumns();
    }
}
