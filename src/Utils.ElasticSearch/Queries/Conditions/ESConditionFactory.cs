using Utils.ElasticSearch.Queries.Abstractions;
using Utils.Util;
using Utils.Util.Helpers;
using Nest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Operator = Utils.Util.Datas.Queries.Operator;

namespace Utils.ElasticSearch.Queries.Conditions
{
    /// <summary>
    /// Sql查询条件工厂
    /// </summary>
    public static class ESConditionFactory
    {
        /// <summary>
        /// 创建Sql查询条件
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        /// <param name="operator">操作符</param>
        public static Abstractions.ICondition Create<TEntity>(QueryContainerDescriptor<TEntity> queryContainerDescriptor,string column, object obj, Operator @operator) where TEntity:class
        {
            var func = Lambda.CreateExpression<TEntity>(column);
            switch (@operator)
            {
                case Operator.Equal:
                    if(obj==null)
                    {
                        return new MississCondition(new ExistsCondition<TEntity>(queryContainerDescriptor,column));
                    }
                    return new TermCondition<TEntity>(queryContainerDescriptor, GetFuncObject<TEntity>(column), obj);
                case Operator.NotEqual:
                    if (obj == null)
                    {
                        return new ExistsCondition<TEntity>(queryContainerDescriptor, column);
                    }
                    var left= new TermCondition<TEntity>(queryContainerDescriptor, GetFuncObject<TEntity>(column), obj);
                    return new MustNotCondition(left);
                case Operator.Greater:
                case Operator.Less:
                case Operator.GreaterEqual:
                case Operator.LessEqual:
                    var type= obj.GetType();
                    type=Common.GetType(type);
                    return GetRangeCondition(type, queryContainerDescriptor, GetFuncObject<TEntity>(column), obj, @operator);
                case Operator.Contains:
                    return new MatchPhraseCondition<TEntity>(queryContainerDescriptor , GetFuncObject<TEntity>(column), obj);
                case Operator.Starts:
                    return new PrefixCondition<TEntity>(queryContainerDescriptor, GetFuncObject<TEntity>(column), obj);
                case Operator.In:
                    return CreateInCondition<TEntity>(queryContainerDescriptor, column, obj as IEnumerable);
                case Operator.NotIn:
                    return CreateInCondition<TEntity>(queryContainerDescriptor, column, obj as IEnumerable,true);
                //case Operator.Ends:
                //    return new LikeCondition(left, right); //SUFFIX
            }
            throw new NotImplementedException($"运算符 {@operator.Description()} 未实现");
        }

        private static Expression<Func<TEntity,object>> GetFuncObject<TEntity>(string column)
        {
            return Lambda.CreateExpression<TEntity>(column);
        }

        /// <summary>
        /// 创建In or not in条件
        /// </summary>
        private static Abstractions.ICondition CreateInCondition<TEntity>(QueryContainerDescriptor<TEntity> queryContainerDescriptor,string column, IEnumerable values, bool notIn = false) where TEntity:class
        {
            if (values == null)
                return NullCondition.Instance;

            var exp = GetFuncObject<TEntity>(column);

            var list = new List<object>();
            foreach (var m in values)
            {
                list.Add(m);
            }

            var left = new TermsCondition<TEntity>(queryContainerDescriptor, exp, list);

            if (notIn)
                return new MustNotCondition(left);

            return left;
        }

        private static Abstractions.ICondition GetRangeCondition<TEntity>(Type type, QueryContainerDescriptor<TEntity> queryContainerDescriptor, Expression<Func<TEntity, object>> expField,object obj, Operator @operator) where TEntity:class
        {
            switch (type.Name)
            {
                case "DateTime":
                    return new DateRangeCondition<TEntity>(queryContainerDescriptor, expField, (DateTime)obj, @operator);
                case "Int32":
                case "Int64":
                case "Double":
                    return new NumericRangeCondition<TEntity>(queryContainerDescriptor, expField, Util.Helpers.Convert.ToDouble(obj), @operator);
            }
            throw new NotImplementedException($"运算符 {@operator.Description()} 类型 {type.Name} 未实现");
        }
    }
}
