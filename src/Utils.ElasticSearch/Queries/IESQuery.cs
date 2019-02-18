using Utils.ElasticSearch.Queries.Abstractions;
using Utils.ElasticSearch.Queries.Core;
using Utils.Util.Datas.Queries;
using Utils.Util.Datas.Repositories;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ElasticSearch.Queries
{
    public interface IESQuery<TEntity> where TEntity : class
    {
        /// <summary>
        /// 清空并初始化
        /// </summary>
        void Clear();
        /// <summary>
        /// 获取查询Json
        /// </summary>
        /// <returns></returns>
        string GetJson();
        
        /// <summary>
        /// ES 查询名生成器
        /// </summary>
        /// <returns></returns>
        IESBuilder<TEntity> NewBuilder();

        /// <summary>
        /// ES 查询原生Response
        /// </summary>
        /// <returns></returns>
        Nest.ISearchResponse<TEntity> ToResponse();
        /// <summary>
        /// ES 查询原生Response
        /// </summary>
        /// <returns></returns>
        Task<Nest.ISearchResponse<TEntity>> ToResponseAsync();
        /// <summary>
        /// ES 查询获取实体
        /// </summary>
        /// <returns></returns>
        TEntity To();
        /// <summary>
        /// ES 查询获取实体
        /// </summary>
        /// <returns></returns>
        Task<TEntity> ToAsync();
        /// <summary>
        /// ES 查询获取列表
        /// </summary>
        /// <returns></returns>
        List<TEntity> ToList();
        /// <summary>
        /// ES 查询获取列表
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> ToListAsync();
        /// <summary>
        /// ES 查询获取分页列表
        /// </summary>
        /// <returns></returns>
        PagerList<TEntity> ToPagerList();
        /// <summary>
        /// ES 查询获取分页列表
        /// </summary>
        /// <returns></returns>
        Task<PagerList<TEntity>> ToPagerListAsync();
        /// <summary>
        /// ES 查询获取聚合字典
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ToDict();
        /// <summary>
        /// ES 查询获取聚合字典
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, object>> ToDictAsync();
        /// <summary>
        /// ES 查询获取原生AggregateDictionary字典
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<string, IAggregate> ToAggregateDictionary();
        /// <summary>
        /// ES 查询获取原生AggregateDictionary字典
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyDictionary<string, IAggregate>> ToAggregateDictionaryAsync();
        /// <summary>
        /// ES Type字段
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        IESQuery<TEntity> Field(string columns);
        /// <summary>
        /// ES Type字段
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IESQuery<TEntity> Field(Expression<Func<TEntity, object[]>> expression);
        /// <summary>
        /// ES Type字段
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IESQuery<TEntity> Field(Expression<Func<TEntity, object>> expression);
        /// <summary>
        /// ES Index
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        IESQuery<TEntity> Index(string table);
        /// <summary>
        /// ES Index
        /// </summary>
        /// <returns></returns>
        IESQuery<TEntity> Index();
        /// <summary>
        /// ES Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IESQuery<TEntity> Type(string type);
        /// <summary>
        /// ES 查询记录返回起始位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IESQuery<TEntity> Skip(int index);
        /// <summary>
        /// ES 查询记录返回数量条数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IESQuery<TEntity> Take(int count);
        /// <summary>
        /// ES 查询记录分页设置
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IESQuery<TEntity> Page(int pageIndex, int pageSize = 10);
        /// <summary>
        /// ES 查询记录排序，升序
        /// 如字段是"Text"类型，是不能进行排序的，请需要使用"field.keyword"，进行排序
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IESQuery<TEntity> SortAsc(Expression<Func<TEntity, object>> expression);
        /// <summary>
        /// ES 查询记录排序，降序
        /// 如字段是"Text"类型，是不能进行排序的，请需要使用"field.keyword"，进行排序
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IESQuery<TEntity> SortDesc(Expression<Func<TEntity, object>> expression);
        /// <summary>
        /// ES 查询记录排序，升序
        /// 支持字段"Text"类型排序，请手动设置字段"field.keyword"
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IESQuery<TEntity> SortAsc(string field);
        /// <summary>
        /// ES 查询记录排序，降序
        /// 支持字段"Text"类型排序，请手动设置字段"field.keyword"
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IESQuery<TEntity> SortDesc(string field);
        /// <summary>
        /// ES 查询记录排序
        /// 如字段是"Text"类型，是不能进行排序的，请需要使用"field.keyword"，进行排序
        /// 
        /// isAsc=true,升序
        /// isAsc=false,降序
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        IESQuery<TEntity> Sort(Expression<Func<TEntity, object>> expression, bool isAsc);
        /// <summary>
        /// ES 查询条件
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IESQuery<TEntity> Where(Expression<Func<TEntity, bool>> expression);
        /// <summary>
        /// ES 查询条件,when condition
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        IESQuery<TEntity> WhereIf(Expression<Func<TEntity, bool>> expression, bool condition);
        /// <summary>
        /// ES 查询条件 Must
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IESQuery<TEntity> Must(Abstractions.ICondition condition);
        /// <summary>
        /// ES 查询条件 Should
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IESQuery<TEntity> Should(Abstractions.ICondition condition);
        /// <summary>
        /// ES 查询条件 MustNot
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IESQuery<TEntity> MustNot(Abstractions.ICondition condition);
        /// <summary>
        /// ES 查询全文搜索
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IESQuery<TEntity> Match(Expression<Func<TEntity, object>> expression, string value);
        /// <summary>
        /// ES 查询全文搜索,when condition
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        IESQuery<TEntity> MatchIf(Expression<Func<TEntity, object>> expression, string value, bool condition);
        /// <summary>
        /// ES 查询全文搜索 MatchAll
        /// </summary>
        /// <returns></returns>
        IESQuery<TEntity> MatchAll();
        /// <summary>
        /// ES 查询Ids
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        IESQuery<TEntity> Ids(string[] values);
        /// <summary>
        /// ES 查询 通配符
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IESQuery<TEntity> Wildcard(Expression<Func<TEntity, object>> expression, string value);
        /// <summary>
        /// ES 查询 正则
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IESQuery<TEntity> Regexp(Expression<Func<TEntity, object>> expression, string value);
        /// <summary>
        ///  ES 查询字段
        ///  field.keyword
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IESQuery<TEntity> TextKeyword(Expression<Func<TEntity, object>> expression, string value);
        /// <summary>
        /// ES 查询模糊
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="maxExpansions"></param>
        /// <param name="prefixLength"></param>
        /// <param name="transpositions"></param>
        /// <returns></returns>
        IESQuery<TEntity> Fuzzy(Expression<Func<TEntity, object>> expression, string value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false);
        /// <summary>
        /// ES 查询模糊
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="maxExpansions"></param>
        /// <param name="prefixLength"></param>
        /// <param name="transpositions"></param>
        /// <param name="fuzziness"></param>
        /// <returns></returns>
        IESQuery<TEntity> FuzzyNumeric(Expression<Func<TEntity, object>> expression, double value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, double fuzziness = 2);
        /// <summary>
        /// ES 查询模糊
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="maxExpansions"></param>
        /// <param name="prefixLength"></param>
        /// <param name="transpositions"></param>
        /// <param name="fuzziness"></param>
        /// <returns></returns>
        IESQuery<TEntity> FuzzyDate(Expression<Func<TEntity, object>> expression, DateTime value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, TimeSpan? fuzziness = null);
        /// <summary>
        /// ES 查询模糊
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IESQuery<TEntity> Term(Expression<Func<TEntity, object>> expression, object value);
        /// <summary>
        /// ES 查询 Terms
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        IESQuery<TEntity> Terms(Expression<Func<TEntity, object>> expression, IEnumerable<object> values);
        /// <summary>
        /// ES 查询区间
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        IESQuery<TEntity> Range(Expression<Func<TEntity, object>> expression, object minValue, object maxValue, Boundary boundary = Boundary.Neither);
        /// <summary>
        /// ES 聚合
        /// </summary>
        /// <param name="aggsFunc"></param>
        /// <returns></returns>
        IESQuery<TEntity> Aggs(Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> aggsFunc);
    }
}
