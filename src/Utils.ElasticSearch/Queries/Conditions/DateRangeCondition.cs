using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Operator = Utils.Util.Datas.Queries.Operator;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class DateRangeCondition<TEntity> : Abstractions.ICondition where TEntity : class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<DateRangeQueryDescriptor<TEntity>, IDateRangeQuery> _expression;


        public DateRangeCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity, object>> expField, DateTime value, Operator @operator)
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
            return _queryContainerDescriptor.DateRange(_expression);
        }
    }
}
