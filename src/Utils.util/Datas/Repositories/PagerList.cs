using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Util.Datas.Repositories
{
    /// <summary>
    /// 分页集合
    /// </summary>
    /// <typeparam name="TEntity">元素类型</typeparam>
    public class PagerList<TEntity> : IPagerBase
    {
        /// <summary>
        /// 初始化分页集合
        /// </summary>
        public PagerList() : this(0)
        {
        }

        /// <summary>
        /// 初始化分页集合
        /// </summary>
        /// <param name="data">内容</param>
        public PagerList(IEnumerable<TEntity> data = null)
            : this(0, data)
        {
        }

        /// <summary>
        /// 初始化分页集合
        /// </summary>
        /// <param name="totalCount">总行数</param>
        /// <param name="data">内容</param>
        public PagerList(int totalCount, IEnumerable<TEntity> data = null)
            : this(1, 20, totalCount, data)
        {
        }

        /// <summary>
        /// 初始化分页集合
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">每页显示行数</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="data">内容</param>
        public PagerList(int page, int pageSize, int totalCount, IEnumerable<TEntity> data = null)
        {
            Data = data?.ToList() ?? new List<TEntity>();
            var pager = new Pager(page, pageSize, totalCount);
            TotalCount = pager.TotalCount;
            PageCount = pager.GetPageCount();
            Page = pager.Page;
            PageSize = pager.PageSize;
        }

        /// <summary>
        /// 初始化分页集合
        /// </summary>
        /// <param name="pager">查询对象</param>
        /// <param name="data">内容</param>
        public PagerList(IPager pager, IEnumerable<TEntity> data = null)
            : this(pager.Page, pager.PageSize, pager.TotalCount, data)
        {
        }

        /// <summary>
        /// 页索引，即第几页，从1开始
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页显示行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public List<TEntity> Data { get; }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">索引</param>
        public TEntity this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item">元素</param>
        public void Add(TEntity item)
        {
            Data.Add(item);
        }

        /// <summary>
        /// 添加元素集合
        /// </summary>
        /// <param name="collection">元素集合</param>
        public void AddRange(IEnumerable<TEntity> collection)
        {
            Data.AddRange(collection);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            Data.Clear();
        }

        /// <summary>
        /// 转换分页集合
        /// </summary>
        /// <typeparam name="TResult">目标元素类型</typeparam>
        /// <param name="converter">转换方法</param>
        public PagerList<TResult> Convert<TResult>(Func<TEntity, TResult> converter)
        {
            return Convert(this.Data.Select(converter));
        }

        /// <summary>
        /// 转换分页集合
        /// </summary>
        /// <param name="data">内容</param>
        public PagerList<TResult> Convert<TResult>(IEnumerable<TResult> data)
        {
            return new PagerList<TResult>(Page, PageSize, TotalCount, data);
        }
    }
}
