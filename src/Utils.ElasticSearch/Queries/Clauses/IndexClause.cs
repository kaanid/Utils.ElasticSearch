using Utils.ElasticSearch.Queries.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.ElasticSearch.Queries.Clauses
{
    public class IndexClause : IIndexClause
    {
        private readonly List<string> list;

        public IndexClause()
        {
            list = new List<string>();
        }

        public void Index(string table)
        {
            if (string.IsNullOrWhiteSpace(table))
                throw new ArgumentNullException(nameof(table));

            foreach (var m in table.Split(','))
                list.Add(m);
        }

        public void Index<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            list.Add(type.Name.ToLower());
        }

        public IEnumerable<string> GetIndexs()
        {
            return list;
        }
    }
}
