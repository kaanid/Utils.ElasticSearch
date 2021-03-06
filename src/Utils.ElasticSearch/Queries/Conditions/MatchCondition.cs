﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class MatchCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<MatchQueryDescriptor<TEntity>, IMatchQuery> _expression;

        public MatchCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity, object>> expField, object value)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _expression = (mqd)=> mqd.Field(expField).Query(value.ToString());
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Match(_expression);
        }
    }
}
