using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class WildcardCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<WildcardQueryDescriptor<TEntity>, IWildcardQuery> _expression;

        public WildcardCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity, object>> expField, object value)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _expression = (mqd) => mqd.Field(expField).Value(value);
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Wildcard(_expression);
        }
    }
}
