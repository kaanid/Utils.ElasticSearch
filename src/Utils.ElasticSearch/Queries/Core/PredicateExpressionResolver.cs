using Utils.ElasticSearch.Queries.Abstractions;
using Utils.ElasticSearch.Queries.Conditions;
using Utils.ElasticSearch.Queries.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Core
{
    /// <summary>
    /// 谓词表达式解析器
    /// </summary>
    public class PredicateExpressionResolver<TEntiy> where TEntiy:class
    {
        /// <summary>
        /// 辅助操作
        /// </summary>
        private readonly Helper<TEntiy> _helper;
        private Nest.QueryContainerDescriptor<TEntiy> _queryContainerDescriptor = null;

        /// <summary>
        /// 初始化谓词表达式解析器
        /// </summary>
        /// <param name="dialect">方言</param>
        /// <param name="resolver">实体解析器</param>
        /// <param name="register">实体别名注册器</param>
        /// <param name="parameterManager">参数管理器</param>
        public PredicateExpressionResolver(IEntityResolver resolver, Nest.QueryContainerDescriptor<TEntiy> queryContainerDescriptor)
        {
            _helper = new Helper<TEntiy>(resolver, queryContainerDescriptor);
            _queryContainerDescriptor = queryContainerDescriptor;
        }

        /// <summary>
        /// 解析谓词表达式
        /// </summary>
        /// <typeparam name="TEntiy">实体类型</typeparam>
        /// <param name="expression">谓词表达式</param>
        public Abstractions.ICondition Resolve(Expression<Func<TEntiy, bool>> expression)
        {
            if (expression == null)
                return NullCondition.Instance;
            return ResolveExpression(expression, typeof(TEntiy));
        }

        /// <summary>
        /// 解析谓词表达式
        /// </summary>
        private Abstractions.ICondition ResolveExpression(Expression expression, Type type)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return ResolveExpression(((LambdaExpression)expression).Body, type);
                case ExpressionType.OrElse:
                    return ResolveOrExpression((BinaryExpression)expression, type);
                case ExpressionType.AndAlso:
                    return ResolveAndExpression((BinaryExpression)expression, type);
                case ExpressionType.Not:
                    return ResolveNotExpression((UnaryExpression)expression, type);
                default:
                    return _helper.CreateCondition(expression, type);
            }
        }

        /// <summary>
        /// 解析Or表达式
        /// </summary>
        private Abstractions.ICondition ResolveNotExpression(UnaryExpression expression, Type type)
        {
            var asd = expression.Operand;
            Abstractions.ICondition left = ResolveExpression(asd, type);
            return new Utils.ElasticSearch.Queries.Conditions.MustNotCondition(left);
        }

        /// <summary>
        /// 解析Or表达式
        /// </summary>
        private Abstractions.ICondition ResolveOrExpression(BinaryExpression expression, Type type)
        {
            Abstractions.ICondition left = ResolveExpression(expression.Left, type);
            Abstractions.ICondition right = ResolveExpression(expression.Right, type);
            return new Utils.ElasticSearch.Queries.Conditions.ShouldCondition(left, right);
        }

        /// <summary>
        /// 解析And表达式
        /// </summary>
        private Abstractions.ICondition ResolveAndExpression(BinaryExpression expression, Type type)
        {

            Abstractions.ICondition left = ResolveExpression(expression.Left, type);

            Abstractions.ICondition right = ResolveExpression(expression.Right, type);
            return new Utils.ElasticSearch.Queries.Conditions.MustCondition(left, right);
        }
    }
}
