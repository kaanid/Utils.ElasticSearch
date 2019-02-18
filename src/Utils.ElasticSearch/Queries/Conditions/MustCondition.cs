using Utils.ElasticSearch.Queries.Abstractions;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Conditions
{
    public class MustCondition : Abstractions.ICondition
    {
        Abstractions.ICondition _left;
        Abstractions.ICondition _right;
        public MustCondition(Abstractions.ICondition left, Abstractions.ICondition right)
        {
            _left = left;
            _right = right;
        }

        public QueryContainer GetContainer()
        {
            if (_left == null)
                return _right.GetContainer();

            if (_right == null)
                return _left.GetContainer();

            return _left.GetContainer() & _right.GetContainer();
        }
    }
}
