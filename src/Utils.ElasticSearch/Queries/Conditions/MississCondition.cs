using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class MississCondition: Abstractions.ICondition
    {
        private Abstractions.ICondition _condition;

        public MississCondition(Abstractions.ICondition condition)
        {
            _condition = condition;
        }

        public QueryContainer GetContainer()
        {
            return !_condition.GetContainer();
        }
    }
}
