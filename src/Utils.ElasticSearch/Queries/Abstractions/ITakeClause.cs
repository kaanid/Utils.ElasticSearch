using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface ITakeClause
    {
        int Count { get; }
        void Take(int count);
    }
}
