﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Operator = Utils.Util.Datas.Queries.Operator;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class LongRangeCondition<TEntity> : Abstractions.ICondition where TEntity : class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<LongRangeQueryDescriptor<TEntity>, ILongRangeQuery> _expression;
        
        public LongRangeCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity, object>> expField, long value, Operator @operator)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            switch (@operator)
            {

                case Operator.Greater:
                    _expression = qd => qd.Field(expField).GreaterThan(value);
                    break;
                case Operator.GreaterEqual:
                    _expression = qd => qd.Field(expField).GreaterThanOrEquals(value);
                    break;
                case Operator.Less:
                    _expression = qd => qd.Field(expField).LessThan(value);
                    break;
                case Operator.LessEqual:
                    _expression = qd => qd.Field(expField).LessThanOrEquals(value);
                    break;
                default:
                    throw new NotImplementedException($"{@operator} 未实现");
            }
        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.LongRange(_expression);
        }
    }
}
