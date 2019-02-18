using Utils.ElasticSearch.Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Clauses
{
    public class SkipClause : ISkipClause
    {
        public int Index { get; private set; }
        public void Skip(int index)
        {
            Index = index;
        }
    }
}
