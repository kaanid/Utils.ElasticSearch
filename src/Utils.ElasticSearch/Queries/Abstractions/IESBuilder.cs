using Utils.Util.Datas.Queries;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Abstractions
{
    public interface IESBuilder<TEntity> where TEntity:class
    {
        void Clear();

        IESBuilder<TEntity> New();

        string ToJson();

        Func<SearchDescriptor<TEntity>, ISearchRequest> ToFunc();

        ISearchRequest ToRequest();

        IEnumerable<string> GetField();

        IEnumerable<string> GetIndexs();
        QueryContainer GetWhere();

        int GetSkip();

        int GetTake();

        string GetIndexType();

        SortDescriptor<TEntity> GetSort();

        Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> GetAggs();

        IESBuilder<TEntity> Field(string columns);

        IESBuilder<TEntity> Field(Expression<Func<TEntity, object[]>> expression);

        IESBuilder<TEntity> Field(Expression<Func<TEntity, object>> expression);

        IESBuilder<TEntity> Index(string table);

        IESBuilder<TEntity> Type(string type);

        IESBuilder<TEntity> Index();

        IESBuilder<TEntity> Skip(int index);

        IESBuilder<TEntity> Take(int count);

        IESBuilder<TEntity> Page(int pageIndex, int pageSize = 10);

        IESBuilder<TEntity> SortAsc(Expression<Func<TEntity, object>> expression);
        IESBuilder<TEntity> SortDesc(Expression<Func<TEntity, object>> expression);
        IESBuilder<TEntity> SortAsc(string field);
        IESBuilder<TEntity> SortDesc(string field);
        IESBuilder<TEntity> Sort(Expression<Func<TEntity, object>> expression, bool isAsc);

        IESBuilder<TEntity> Where(Expression<Func<TEntity, bool>> expression);
        IESBuilder<TEntity> WhereIf(Expression<Func<TEntity, bool>> expression, bool condition);
        IESBuilder<TEntity> Must(ICondition condition);

        IESBuilder<TEntity> Should(ICondition condition);

        IESBuilder<TEntity> MustNot(ICondition condition);

        IESBuilder<TEntity> Match(Expression<Func<TEntity, object>> expression, string value);

        IESBuilder<TEntity> MatchIf(Expression<Func<TEntity, object>> expression, string value, bool condition);

        IESBuilder<TEntity> MatchAll();

        IESBuilder<TEntity> Ids(string[] values);

        IESBuilder<TEntity> Wildcard(Expression<Func<TEntity, object>> expression, string value);

        IESBuilder<TEntity> Regexp(Expression<Func<TEntity, object>> expression, string value);

        IESBuilder<TEntity> TextKeyword(Expression<Func<TEntity, object>> expression, string value);

        IESBuilder<TEntity> Fuzzy(Expression<Func<TEntity, object>> expression, string value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false);

        IESBuilder<TEntity> FuzzyNumeric(Expression<Func<TEntity, object>> expression, double value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, double fuzziness = 2);

        IESBuilder<TEntity> FuzzyDate(Expression<Func<TEntity, object>> expression, DateTime value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, TimeSpan? fuzziness = null);

        IESBuilder<TEntity> Term(Expression<Func<TEntity, object>> expression, object value);

        IESBuilder<TEntity> Terms(Expression<Func<TEntity, object>> expression, IEnumerable<object> values);

        IESBuilder<TEntity> Range(Expression<Func<TEntity, object>> expression, object minValue, object maxValue, Boundary boundary = Boundary.Neither);

        IESBuilder<TEntity> Aggs(Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> aggregationsSelector);

        
    }
}
