using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class RegexpCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<RegexpQueryDescriptor<TEntity>, IRegexpQuery> _expression;

        public RegexpCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity, object>> expField, string value)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _expression = (mqd) => mqd.Field(expField).Value(value);
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Regexp(_expression);
        }
    }
}
