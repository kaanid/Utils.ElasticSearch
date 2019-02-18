using Utils.ElasticSearch.Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Clauses
{
    public class TypeClause: ITypeClause
    {
        public string IndexType { get; private set; }

        public void Type(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException(nameof(type));

            IndexType = type.ToLower();
        }
    }
}
