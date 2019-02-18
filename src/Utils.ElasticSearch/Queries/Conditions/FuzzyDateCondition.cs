using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class FuzzyDateCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<FuzzyDateQueryDescriptor<TEntity>, IFuzzyDateQuery> _selector;

        public FuzzyDateCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor,Expression<Func<TEntity,object>> expFiled, DateTime value,int maxExpansions=100,int prefixLength=2,bool transpositions=false, TimeSpan? fuzziness = null)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            fuzziness = fuzziness ?? TimeSpan.FromDays(2);
            _selector = _ => _.Field(expFiled).Value(value).MaxExpansions(maxExpansions).PrefixLength(prefixLength).Transpositions(transpositions).Fuzziness(fuzziness.Value);

        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.FuzzyDate(_selector);
        }
    }
}
