﻿using Utils.ElasticSearch.Queries.Abstractions;
using Utils.Util;
using Utils.Util.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Core
{
    /// <summary>
    /// 实体解析器
    /// </summary>
    public class EntityResolver : IEntityResolver
    {

        /// <summary>
        /// 初始化实体解析器
        /// </summary>
        /// <param name="matedata">实体元数据</param>
        public EntityResolver()
        {

        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="entity">实体类型</param>
        public string GetTable(Type entity)
        {
            return entity.Name;
        }

        /// <summary>
        /// 获取架构
        /// </summary>
        /// <param name="entity">实体类型</param>
        public string GetSchema(Type entity)
        {
            return null;
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="columns">列名表达式</param>
        /// <param name="propertyAsAlias">是否将属性名映射为列别名</param>
        public string GetColumns<TEntity>(Expression<Func<TEntity, object[]>> columns, bool propertyAsAlias)
        {
            var names = Lambda.GetLastNames(columns);
            return names.Join();
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        public string GetColumn<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            return GetExpressionColumn<TEntity>(expression);
        }

        /// <summary>
        /// 获取表达式列名
        /// </summary>
        private string GetExpressionColumn<TEntity>(Expression expression)
        {
            if (expression == null)
                return null;
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return GetExpressionColumn<TEntity>(((LambdaExpression)expression).Body);
                case ExpressionType.Convert:
                case ExpressionType.MemberAccess:
                    return GetSingleColumn<TEntity>(expression);
                case ExpressionType.ListInit:
                    var isDictionary = typeof(Dictionary<object, string>).GetGenericTypeDefinition().IsAssignableFrom(expression.Type.GetGenericTypeDefinition());
                    return isDictionary ? GetDictionaryColumns<TEntity>((ListInitExpression)expression) : null;
            }
            return null;
        }

        /// <summary>
        /// 获取单列
        /// </summary>
        private string GetSingleColumn<TEntity>(Expression expression)
        {
            //var name = Lambda.GetLastName(expression);
            //return name;

            return GetColumn(expression);
        }

        /// <summary>
        /// 获取字典多列
        /// </summary>
        private string GetDictionaryColumns<TEntity>(ListInitExpression expression)
        {
            var dictionary = GetDictionaryByListInitExpression(expression);
            return GetColumns(dictionary);
        }

        /// <summary>
        /// 获取字典
        /// </summary>
        private IDictionary<object, string> GetDictionaryByListInitExpression(ListInitExpression expression)
        {
            var result = new Dictionary<object, string>();
            foreach (var elementInit in expression.Initializers)
            {
                var keyValue = GetKeyValue(elementInit.Arguments);
                if (keyValue == null)
                    continue;
                var item = keyValue.SafeValue();
                result.Add(item.Key, item.Value);
            }
            return result;
        }

        /// <summary>
        /// 获取键值对
        /// </summary>
        private KeyValuePair<object, string>? GetKeyValue(IEnumerable<Expression> arguments)
        {
            if (arguments == null)
                return null;
            var list = arguments.ToList();
            if (list.Count < 2)
                return null;
            return new KeyValuePair<object, string>(Lambda.GetName(list[0]), Lambda.GetValue(list[1]).SafeString());
        }

        /// <summary>
        /// 通过字典创建列
        /// </summary>
        private string GetColumns(IDictionary<object, string> dictionary)
        {
            string result = null;
            foreach (var item in dictionary)
                result += $"{item.Key} As {item.Value},";
            return result?.TrimEnd(',');
        }

        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="entity">实体类型</param>
        /// <param name="right">是否取右侧操作数</param>
        public string GetColumn(Expression expression, Type entity, bool right = false)
        {
            //var column = Lambda.GetLastName(expression, right);
            //return column;
            return GetColumn(expression);
        }

        public string GetColumn(Expression expression)
        {
            return Lambda.GetName(expression);
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="right">是否取右侧操作数</param>
        public Type GetType(Expression expression, bool right = false)
        {
            var memberExpression = Lambda.GetMemberExpression(expression, right);
            return memberExpression?.Expression?.Type;
        }
    }
}