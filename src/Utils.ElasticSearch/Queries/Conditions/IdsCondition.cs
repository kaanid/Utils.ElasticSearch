using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    /// <summary>
    /// ids
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class IdsCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<IdsQueryDescriptor, IIdsQuery> _expression;

        public IdsCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor,string[] values)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _expression = (mqd) => mqd.Values(values);
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Ids(_expression);
        }
    }
}
