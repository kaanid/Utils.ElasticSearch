using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class PrefixCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Expression<Func<TEntity, object>> _expression;
        private string _value;

        public PrefixCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity,object>> expression, object value)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _value = value.ToString();
            _expression = expression;
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Prefix(_expression, _value);
        }
    }
}
