using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch.Queries.Core
{
    /// <summary>
    /// 列集合
    /// </summary>
    public class ColumnCollection
    {
        /// <summary>
        /// 初始化列集合
        /// </summary>
        /// <param name="columns">列集合</param>
        /// <param name="tableAlias">表别名</param>
        /// <param name="table">表实体类型</param>
        /// <param name="raw">使用原始值</param>
        public ColumnCollection(string columns, Type table = null, bool raw = false)
        {
            Columns = columns;
            Table = table;
            Raw = raw;
        }

        /// <summary>
        /// 列集合
        /// </summary>
        public string Columns { get; }


        /// <summary>
        /// 表实体类型
        /// </summary>
        public Type Table { get; }

        /// <summary>
        /// 使用原始值
        /// </summary>
        public bool Raw { get; }

        /// <summary>
        /// 获取列名列表
        /// </summary>
        /// <param name="dialect">Sql方言</param>
        /// <param name="register">实体别名注册器</param>
        public string[] ToColumns()
        {
            if (Raw)
                return new string[] { Columns };

            return Columns.Split(',');
        }
    }
}
