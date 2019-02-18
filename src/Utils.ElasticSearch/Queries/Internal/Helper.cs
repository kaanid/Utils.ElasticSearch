using Utils.ElasticSearch.Queries.Abstractions;
using Utils.ElasticSearch.Queries.Conditions;
using Utils.Util;
using Utils.Util.Datas.Queries;
using Utils.Util.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Internal
{
    /// <summary>
    /// Sql生成器辅助操作
    /// </summary>
    internal class Helper<TEntity> where TEntity :class
    {
        /// <summary>
        /// 实体解析器
        /// </summary>
        private readonly IEntityResolver _resolver;
        private readonly Nest.QueryContainerDescriptor<TEntity> _queryContainerDescriptor;

        /// <summary>
        /// 初始化Sql生成器辅助操作
        /// </summary>
        /// <param name="dialect">方言</param>
        /// <param name="resolver">实体解析器</param>
        /// <param name="register">实体别名注册器</param>
        /// <param name="parameterManager">参数管理器</param>
        public Helper(IEntityResolver resolver, Nest.QueryContainerDescriptor<TEntity> queryContainerDescriptor)
        {

            _resolver = resolver;
            _queryContainerDescriptor = queryContainerDescriptor;
        }

        /// <summary>
        /// 获取处理后的列名
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">实体类型</param>
        public string GetColumn(Expression expression, Type type)
        {
            return GetColumn(_resolver.GetColumn(expression, type), type);
        }

        /// <summary>
        /// 获取处理后的列名
        /// </summary>
        /// <param name="expression">列名表达式</param>
        public string GetColumn(Expression<Func<TEntity, object>> expression)
        {
            return GetColumn(_resolver.GetColumn(expression), typeof(TEntity));
        }

        /// <summary>
        /// 获取处理后的列名
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="type">实体类型</param>
        public string GetColumn(string column, Type type)
        {
            return column;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns>表达式</returns>
        public object GetValue(Expression expression)
        {
            var result = Lambda.GetValue(expression);
            if (result == null)
                return null;
            var type = result.GetType();
            if (type.IsEnum)
                return Util.Helpers.Enum.GetValue(type, result);
            return result;
        }

        /// <summary>
        /// 创建查询条件并添加参数
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="type">实体类型</param>
        public Abstractions.ICondition CreateCondition(Expression expression, Type type)
        {
            return CreateCondition(GetColumn(expression, type), GetValue(expression), Lambda.GetOperator(expression).SafeValue());
        }

        /// <summary>
        /// 创建查询条件并添加参数
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        public Abstractions.ICondition CreateCondition(string column, object value, Operator @operator)
        {
            if (string.IsNullOrWhiteSpace(column))
                throw new ArgumentNullException(nameof(column));
            
            if (IsInCondition(@operator, value))
                @operator = Operator.In;
            
            if (IsNotInCondition(@operator, value))
                @operator = Operator.NotIn;

            return ESConditionFactory.Create<TEntity>(_queryContainerDescriptor, column, value,@operator);
        }

        /// <summary>
        /// 是否In条件
        /// </summary>
        private bool IsInCondition(Operator @operator, object value)
        {
            if (@operator == Operator.In)
                return true;
            if (@operator == Operator.Contains && value != null && Reflection.IsCollection(value.GetType()))
                return true;
            return false;
        }

        /// <summary>
        /// 是否Not In条件
        /// </summary>
        private bool IsNotInCondition(Operator @operator, object value)
        {
            if (@operator == Operator.NotIn)
                return true;
            return false;
        }

        public Operator GetLeftOperator(Boundary boundary)
        {
            return Lambda.GetLeftOperator(boundary);
        }

        /// <summary>
        /// 获取右侧查询操作符
        /// </summary>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public Operator GetRightOperator(Boundary boundary)
        {
            return Lambda.GetRightOperator(boundary);
        }
    }
}
