using Utils.ElasticSearch.Queries.Abstractions;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Conditions
{
    /// <summary>
    /// 空查询条件
    /// </summary>
    public class NullCondition : Abstractions.ICondition
    {
        /// <summary>
        /// 封闭构造方法
        /// </summary>
        private NullCondition()
        {
        }

        /// <summary>
        /// 空查询条件实例
        /// </summary>
        public static readonly NullCondition Instance = new NullCondition();

        /// <summary>
        /// 获取查询条件
        /// </summary>
        public QueryContainer GetContainer()
        {
            return null;
        }
    }
}
