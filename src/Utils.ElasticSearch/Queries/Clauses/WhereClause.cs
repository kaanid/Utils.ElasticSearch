using Utils.ElasticSearch.Queries.Abstractions;
using Utils.ElasticSearch.Queries.Conditions;
using Utils.ElasticSearch.Queries.Internal;
using Utils.Util.Datas.Queries;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ICondition = Utils.ElasticSearch.Queries.Abstractions.ICondition;

namespace Utils.ElasticSearch.Queries.Clauses
{
    public class WhereClause<TEntity> : IWhereClause<TEntity> where TEntity : class
    {
        /// <summary>
        /// 实体解析器
        /// </summary>
        private readonly IEntityResolver _resolver;
        /// <summary>
        /// 辅助操作
        /// </summary>
        private readonly Helper<TEntity> _helper;
        /// <summary>
        /// 谓词表达式解析器
        /// </summary>
        private readonly Utils.ElasticSearch.Queries.Core.PredicateExpressionResolver<TEntity> _expressionResolver;
        /// <summary>
        /// 查询条件
        /// </summary>
        private ICondition _condition;

        private QueryContainerDescriptor<TEntity> _queryContainerDescriptor;

        public WhereClause(IEntityResolver entityResolver, QueryContainerDescriptor<TEntity> queryContainerDescriptor, ICondition condition=null)
        {
            _resolver = entityResolver;
            _condition = condition;
            
            _helper = new Helper<TEntity>(_resolver, queryContainerDescriptor);
            _expressionResolver = new Utils.ElasticSearch.Queries.Core.PredicateExpressionResolver<TEntity>(_resolver,queryContainerDescriptor);

            _queryContainerDescriptor = queryContainerDescriptor;
        }

        public QueryContainer GetContainer()
        {
            if (_condition == null)
                return null;
            return _condition.GetContainer();
        }

        /// <summary>
        /// Must连接条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        public void Must(ICondition condition)
        {
            _condition = new MustCondition(_condition, condition);
        }

        /// <summary>
        /// Should连接条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        public void Should(ICondition condition)
        {
            _condition = new ShouldCondition(_condition, condition);
        }

        /// <summary>
        /// Mustnot
        /// </summary>
        /// <param name="condition"></param>
        public void MustNot(ICondition condition)
        {
            condition = new MustNotCondition(condition);
            Must(condition);
        }

        public void Match(Expression<Func<TEntity, object>> expression,string value)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var condition= new MatchCondition<TEntity>(_queryContainerDescriptor, expression, value);
            Must(condition);
        }

        public void MatchIf(Expression<Func<TEntity, object>> expression, string value, bool condition)
        {
            if (condition)
                Match(expression, value);
        }

        public void MatchAll()
        {
            var condition = new MatchAllCondition<TEntity>(_queryContainerDescriptor);
            Must(condition);
        }

        public void Ids(string[] values)
        {
            var condition = new IdsCondition<TEntity>(_queryContainerDescriptor, values);
            Must(condition);
        }

        public void Where(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            var condition = _expressionResolver.Resolve(expression);

            Must(condition);
        }

        public void WhereIf(Expression<Func<TEntity, bool>> expression, bool condition)
        {
            if (condition)
                Where(expression);
        }

        public void Wildcard(Expression<Func<TEntity, object>> expression, string value)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var condition = new WildcardCondition<TEntity>(_queryContainerDescriptor, expression, value);
            Must(condition);
        }

        public void Regexp(Expression<Func<TEntity, object>> expression, string value)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var condition = new RegexpCondition<TEntity>(_queryContainerDescriptor, expression, value);
            Must(condition);
        }

        public void TextKeyword(Expression<Func<TEntity, object>> expression, string value)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            //var column= _helper.GetColumn(expression);
            var column = _resolver.GetColumn(expression, typeof(TEntity), false);
            var condition = new TextKeywordCondition<TEntity>(_queryContainerDescriptor, column, value);
            Must(condition);
        }

        public void Fuzzy(Expression<Func<TEntity, object>> expression,string value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var condition = new FuzzyCondition<TEntity>(_queryContainerDescriptor, expression, value, maxExpansions, prefixLength, transpositions);
            Must(condition);
        }

        public void FuzzyNumeric(Expression<Func<TEntity, object>> expression, double value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, double fuzziness = 2)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var condition = new FuzzyNumericCondition<TEntity>(_queryContainerDescriptor, expression, value,maxExpansions,prefixLength,transpositions,fuzziness);
            Must(condition);
        }

        public void FuzzyDate(Expression<Func<TEntity, object>> expression,DateTime value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, TimeSpan? fuzziness = null)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var condition = new FuzzyDateCondition<TEntity>(_queryContainerDescriptor, expression,value, maxExpansions, prefixLength, transpositions, fuzziness);
            Must(condition);
        }

        public void Term(Expression<Func<TEntity, object>> expression, object value)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var condition = new TermCondition<TEntity>(_queryContainerDescriptor,expression, value);
            Must(condition);
        }

        public void Terms(Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var condition = new TermsCondition<TEntity>(_queryContainerDescriptor, expression, values);
            Must(condition);
        }

        public void Range(Expression<Func<TEntity, object>> expression, object minValue, object maxValue, Boundary boundary = Boundary.Neither)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var column = _helper.GetColumn(expression);
            if (minValue != null)
            {
                var @operator = _helper.GetLeftOperator(boundary);
                var condition = _helper.CreateCondition(column, minValue, @operator);
                Must(condition);
            }

            if(maxValue!=null)
            {
                var @operator = _helper.GetRightOperator(boundary);
                var condition = _helper.CreateCondition(column, maxValue, @operator);
                Must(condition);
            }
        }
    }
}
