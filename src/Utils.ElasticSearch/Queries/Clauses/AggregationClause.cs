using Utils.ElasticSearch.Queries.Abstractions;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Clauses
{
    public class AggregationClause<TEntity> : IAggregationClause<TEntity> where TEntity:class
    {
        private Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> _aggregationContainer;
        private bool _isSet;

        public void Aggs(Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> aggregationsSelector)
        {
            if (aggregationsSelector == null)
                return;
            _aggregationContainer = aggregationsSelector;
            _isSet = true;
        }
        public Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> GetAggs()
        {
            if (!_isSet)
                return null;

            return _aggregationContainer;
        }
    }
}
