using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface IAggregationClause<TEntity> where TEntity : class
    {
        void Aggs(Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> aggregationsSelector);
        Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> GetAggs();
    }
}
