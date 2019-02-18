using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class ShouldCondition: Abstractions.ICondition
    {
        Abstractions.ICondition _left;
        Abstractions.ICondition _right;
        public ShouldCondition(Abstractions.ICondition left, Abstractions.ICondition right)
        {
            _left = left;
            _right = right;
        }

        public Nest.QueryContainer GetContainer()
        {
            return _left.GetContainer() | _right.GetContainer();
        }
    }
}
