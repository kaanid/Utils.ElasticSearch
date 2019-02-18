using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface ISkipClause
    {
        int Index { get; }
        void Skip(int index);
    }
}
