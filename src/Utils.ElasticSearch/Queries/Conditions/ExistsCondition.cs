using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class ExistsCondition<TEntity> : Abstractions.ICondition where TEntity : class
    {
        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;

        private string _colnum = null;

        public ExistsCondition(QueryContainerDescriptor<TEntity> queryContainerDescriptor, string colnum)
        {
            _queryContainerDescriptor = queryContainerDescriptor;
            _colnum = colnum;
        }

        public QueryContainer GetContainer()
        {
            return _queryContainerDescriptor.Exists(eqd => eqd.Field(_colnum));
        }
    }
}
