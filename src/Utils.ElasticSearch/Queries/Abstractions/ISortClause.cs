using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface ISortClause<TEntity> where TEntity : class
    {
        void SortAsc(Expression<Func<TEntity, object>> expression);
        void SortDesc(Expression<Func<TEntity, object>> expression);
        void SortAsc(string field);
        void SortDesc(string field);
        void Sort(Expression<Func<TEntity, object>> expression, bool isAsc);
        SortDescriptor<TEntity> GetSort();
    }
}
