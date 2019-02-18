using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nest;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class FuzzyNumericCondition<TEntity> : Abstractions.ICondition where TEntity:class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;
        private Func<FuzzyNumericQueryDescriptor<TEntity>, IFuzzyNumericQuery> _selector;

        public FuzzyNumericCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor,Expression<Func<TEntity,object>> expFiled, double value,int maxExpansions=100,int prefixLength=2,bool transpositions=false,double fuzziness=2)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _selector = _ => _.Field(expFiled).Value(value).MaxExpansions(maxExpansions).PrefixLength(prefixLength).Transpositions(transpositions).Fuzziness(fuzziness);

        }
        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.FuzzyNumeric(_selector);
        }
    }
}
