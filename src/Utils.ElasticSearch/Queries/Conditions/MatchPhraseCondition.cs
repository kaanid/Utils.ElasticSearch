using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class MatchPhraseCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<MatchPhraseQueryDescriptor<TEntity>, IMatchPhraseQuery> _expression;

        public MatchPhraseCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity, object>> expField, object value)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _expression = (mqd)=> mqd.Field(expField).Query(value.ToString());
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.MatchPhrase(_expression);
        }
    }
}
