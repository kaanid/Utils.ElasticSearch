using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface IIndexClause
    {
        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="table">表名</param>
        void Index(string table);
        /// <summary>
        /// 设置表名
        /// </summary>
        void Index<TEntity>() where TEntity : class;

        IEnumerable<string> GetIndexs();
    }
}
