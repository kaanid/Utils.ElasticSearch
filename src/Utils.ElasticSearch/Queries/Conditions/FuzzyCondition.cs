using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class FuzzyCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<FuzzyQueryDescriptor<TEntity>, IFuzzyQuery> _selector;

        public FuzzyCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor,Expression<Func<TEntity,object>> expFiled, string value,int maxExpansions=100,int prefixLength=2,bool transpositions=false)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _selector = _ => _.Field(expFiled).Value(value).MaxExpansions(maxExpansions).PrefixLength(prefixLength).Transpositions(transpositions).Fuzziness(Fuzziness.Auto);

        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Fuzzy(_selector);
        }
    }
}
