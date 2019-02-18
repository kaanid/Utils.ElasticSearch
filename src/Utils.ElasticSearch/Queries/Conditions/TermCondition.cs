using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class TermCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<TermQueryDescriptor<TEntity>, ITermQuery> _selector;

        public TermCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor,Expression<Func<TEntity,object>> expression, object value)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _selector = _ => _.Field(expression).Value(value).Verbatim(value != null && value.ToString() == string.Empty);
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Term(_selector);
        }
    }
}
