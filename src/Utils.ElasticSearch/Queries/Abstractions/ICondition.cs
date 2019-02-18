using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface ICondition
    {
        Nest.QueryContainer GetContainer();
    }
}
