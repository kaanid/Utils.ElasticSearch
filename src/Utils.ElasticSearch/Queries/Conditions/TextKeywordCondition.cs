using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class TextKeywordCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<TermQueryDescriptor<TEntity>, ITermQuery> _selector;

        public TextKeywordCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor,string column, object value)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _selector = _ => _.Field($"{column}.keyword").Value(value).Verbatim(value != null && value.ToString() == string.Empty);

        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Term(_selector);
        }
    }
}
