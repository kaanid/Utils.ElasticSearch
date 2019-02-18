using Utils.ElasticSearch.Queries.Abstractions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Clauses
{
    public class SortClause<TEntity> : ISortClause<TEntity> where TEntity : class
    {
        private readonly SortDescriptor<TEntity> sort;

        public SortClause()
        {
            sort = new SortDescriptor<TEntity>();
        }

        public SortDescriptor<TEntity> GetSort()
        {
            return sort;
        }

        public void SortAsc(Expression<Func<TEntity, object>> expression)
        {
            sort.Ascending(expression);
        }

        public void SortAsc(string field)
        {
            sort.Ascending(field);
        }

        public void SortDesc(Expression<Func<TEntity, object>> expression)
        {
            sort.Descending(expression);
        }

        public void SortDesc(string field)
        {
            sort.Descending(field);
        }

        public void Sort(Expression<Func<TEntity, object>> expression, bool isAsc)
        {
            if (isAsc)
            {
                sort.Ascending(expression);
            }
            else
            {
                sort.Descending(expression);
            }
        }
    }
}
