using Utils.ElasticSearch.Queries.Abstractions;
using Utils.ElasticSearch.Queries.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Clauses
{
    public class FieldClause : IFieldClause
    {
        private readonly List<ColumnCollection> _columns;
        private readonly IEntityResolver _resolver;

        public FieldClause(IEntityResolver resolver)
        {
            _resolver = resolver;
            _columns = new List<ColumnCollection>();
        }

        public void Field(string columns, string tableAlias = null)
        {
            if (string.IsNullOrWhiteSpace(columns))
                return;

            _columns.Add(new ColumnCollection(columns));
        }

        public void Field<TEntity>(Expression<Func<TEntity, object[]>> expression, bool propertyAsAlias = false) where TEntity : class
        {
            if (expression == null)
                return;
            _columns.Add(new ColumnCollection(_resolver.GetColumns(expression, propertyAsAlias), table: typeof(TEntity)));
        }

        public void Field<TEntity>(Expression<Func<TEntity, object>> expression, string columnAlias = null) where TEntity : class
        {
            if (expression == null)
                return;

            var column = _resolver.GetColumn(expression);
            _columns.Add(new ColumnCollection(column, table: typeof(TEntity)));
        }

        // <summary>
        /// 获取列名
        /// </summary>
        public IEnumerable<string> GetColumns()
        {
            if (_columns.Count == 0)
                return null;

            var result = new List<string>();
            foreach (var item in _columns)
            {
                result.AddRange(item.ToColumns());
            }
            return result;
        }
    }
}
