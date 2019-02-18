using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class TermsCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<TermsQueryDescriptor<TEntity>, ITermsQuery> _selector;

        public TermsCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity, object>> expField, IEnumerable<object> values)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _selector = tqd => tqd.Field(expField).Terms(values);
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Terms(_selector);
        }
    }
}
