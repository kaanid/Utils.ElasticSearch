using Utils.ElasticSearch.Queries.Abstractions;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class MustNotCondition : Abstractions.ICondition
    {
        private Abstractions.ICondition _left;
        public MustNotCondition(Abstractions.ICondition left)
        {
            _left = left;
        }

        public QueryContainer GetContainer()
        {
            return !_left.GetContainer();
        }
    }
}
