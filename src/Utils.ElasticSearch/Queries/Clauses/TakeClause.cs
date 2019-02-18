using Utils.ElasticSearch.Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Clauses
{
    public class TakeClause : ITakeClause
    {
        public int Count { get; private set; } = 10;
        public void Take(int count)
        {
            Count = count;
        }
    }
}
