using Utils.Util.Datas.Queries;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface IWhereClause<TEntity> : ICondition where TEntity : class
    {
        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        void Where(Expression<Func<TEntity, bool>> expression);

        void WhereIf(Expression<Func<TEntity, bool>> expression,bool condition);

        void Must(ICondition condition);

        void Should(ICondition condition);

        void MustNot(ICondition condition);

        void Match(Expression<Func<TEntity, object>> expression, string value);

        void MatchIf(Expression<Func<TEntity, object>> expression, string value, bool condition);

        void MatchAll();

        void Ids(string[] values);

        void Wildcard(Expression<Func<TEntity, object>> expression, string value);

        void Regexp(Expression<Func<TEntity, object>> expression, string value);

        void TextKeyword(Expression<Func<TEntity, object>> expression, string value);

        void Fuzzy(Expression<Func<TEntity, object>> expression, string value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false);

        void FuzzyNumeric(Expression<Func<TEntity, object>> expression, double value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, double fuzziness = 2);

        void FuzzyDate(Expression<Func<TEntity, object>> expression, DateTime value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, TimeSpan? fuzziness = null);

        void Term(Expression<Func<TEntity, object>> expression, object value);

        void Terms(Expression<Func<TEntity, object>> expression, IEnumerable<object> values);

        void Range(Expression<Func<TEntity, object>> expression, object minValue, object maxValue, Boundary boundary= Boundary.Neither);
    }
}
