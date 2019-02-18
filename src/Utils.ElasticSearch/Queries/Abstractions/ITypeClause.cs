using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface ITypeClause
    {
        string IndexType { get; }
        void Type(string type);
    }
}
