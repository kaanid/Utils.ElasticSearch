using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class MatchAllCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;

        public MatchAllCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.MatchAll();
        }
    }
}
