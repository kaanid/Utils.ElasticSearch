using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utils.ElasticSearch.Queries.Abstractions;
using Utils.Util.Datas.Queries;
using Utils.Util.Datas.Repositories;
using Nest;

namespace Utils.ElasticSearch.Queries
{
    public abstract class ESQueryBase<TEntity> : IESQuery<TEntity> where TEntity : class
    {
        private readonly IElasticSearchClient _client;
        protected IESBuilder<TEntity> Builder { get; }
        public bool _queryedClear;

        protected ESQueryBase(IESBuilder<TEntity> esBuilder, IElasticSearchClient client, bool queryedClear = true)
        {
            Builder = esBuilder;
            _client = client;
            _queryedClear = queryedClear;
        }

        protected void TryQueryedClear()
        {
            if (_queryedClear)
                Clear();
        }

        protected Pager GetPager()
        {
            var skip = Builder.GetSkip();
            var take = Builder.GetTake();
            var pager = new Pager(skip / take + 1, take);
            return pager;
        }

        public void Clear()
        {
            Builder.Clear();
        }

        public IESQuery<TEntity> Field(string columns)
        {
            Builder.Field(columns);
            return this;
        }

        public IESQuery<TEntity> Field(Expression<Func<TEntity, object[]>> expression)
        {
            Builder.Field(expression);
            return this;
        }

        public IESQuery<TEntity> Field(Expression<Func<TEntity, object>> expression)
        {
            Builder.Field(expression);
            return this;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connection">数据库连接</param>
        protected Nest.IElasticClient GetClient()
        {
            var connection = _client?.GetClient();
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            return connection;
        }

        public string GetJson()
        {
            return Builder.ToJson();
        }

        public IESQuery<TEntity> Ids(string[] values)
        {
            Builder.Ids(values);
            return this;
        }

        public IESQuery<TEntity> Index(string table)
        {
            Builder.Index(table);
            return this;
        }

        public IESQuery<TEntity> Index()
        {
            Builder.Index();
            return this;
        }

        public IESQuery<TEntity> Type(string type)
        {
            Builder.Type(type);
            return this;
        }

        public IESQuery<TEntity> Match(Expression<Func<TEntity, object>> expression, string value)
        {
            Builder.Match(expression, value);
            return this;
        }

        public IESQuery<TEntity> MatchIf(Expression<Func<TEntity, object>> expression, string value,bool condition)
        {
            Builder.MatchIf(expression, value,condition);
            return this;
        }

        public IESQuery<TEntity> MatchAll()
        {
            Builder.MatchAll();
            return this;
        }

        public IESQuery<TEntity> Must(Abstractions.ICondition condition)
        {
            Builder.Must(condition);
            return this;
        }

        public IESQuery<TEntity> MustNot(Abstractions.ICondition condition)
        {
            Builder.MustNot(condition);
            return this;
        }

        public IESBuilder<TEntity> NewBuilder()
        {
            return Builder.New();
        }

        public IESQuery<TEntity> Should(Abstractions.ICondition condition)
        {
            Builder.Should(condition);
            return this;
        }

        public IESQuery<TEntity> Skip(int index)
        {
            Builder.Skip(index);
            return this;
        }

        public IESQuery<TEntity> SortAsc(Expression<Func<TEntity, object>> expression)
        {
            Builder.SortAsc(expression);
            return this;
        }

        public IESQuery<TEntity> SortAsc(string field)
        {
            Builder.SortAsc(field);
            return this;
        }

        public IESQuery<TEntity> SortDesc(Expression<Func<TEntity, object>> expression)
        {
            Builder.SortDesc(expression);
            return this;
        }

        public IESQuery<TEntity> SortDesc(string field)
        {
            Builder.SortDesc(field);
            return this;
        }

        public IESQuery<TEntity> Sort(Expression<Func<TEntity, object>> expression, bool isAsc)
        {
            Builder.Sort(expression,isAsc);
            return this;
        }

        public IESQuery<TEntity> Take(int count)
        {
            Builder.Take(count);
            return this;
        }

        public IESQuery<TEntity> Page(int pageIndex,int pageSize=10)
        {
            Builder.Page(pageIndex,pageSize);
            return this;
        }

        public abstract Nest.ISearchResponse<TEntity> ToResponse();
        public abstract Task<Nest.ISearchResponse<TEntity>> ToResponseAsync();
        public abstract TEntity To();

        public abstract Task<TEntity> ToAsync();

        public abstract List<TEntity> ToList();

        public abstract Task<List<TEntity>> ToListAsync();

        public abstract PagerList<TEntity> ToPagerList();
        public abstract Task<PagerList<TEntity>> ToPagerListAsync();

        public abstract Dictionary<string, object> ToDict();

        public abstract Task<Dictionary<string, object>> ToDictAsync();

        public abstract AggregateDictionary ToAggregateDictionary();

        public abstract Task<AggregateDictionary> ToAggregateDictionaryAsync();

        public IESQuery<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            Builder.Where(expression);
            return this;
        }

        public IESQuery<TEntity> WhereIf(Expression<Func<TEntity, bool>> expression, bool condition)
        {
            Builder.WhereIf(expression,condition);
            return this;
        }

        public IESQuery<TEntity> Wildcard(Expression<Func<TEntity, object>> expression, string value)
        {
            Builder.Wildcard(expression, value);
            return this;
        }

        public IESQuery<TEntity> Regexp(Expression<Func<TEntity, object>> expression, string value)
        {
            Builder.Regexp(expression, value);
            return this;
        }

        public IESQuery<TEntity> TextKeyword(Expression<Func<TEntity, object>> expression, string value)
        {
            Builder.TextKeyword(expression, value);
            return this;
        }

        public IESQuery<TEntity> Fuzzy(Expression<Func<TEntity, object>> expression, string value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false)
        {
            Builder.Fuzzy(expression, value, maxExpansions, prefixLength, transpositions);
            return this;
        }

        public IESQuery<TEntity> FuzzyNumeric(Expression<Func<TEntity, object>> expression, double value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, double fuzziness = 2)
        {
            Builder.FuzzyNumeric(expression, value, maxExpansions, prefixLength, transpositions, fuzziness);
            return this;
        }

        public IESQuery<TEntity> FuzzyDate(Expression<Func<TEntity, object>> expression, DateTime value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, TimeSpan? fuzziness = null)
        {
            Builder.FuzzyDate(expression, value, maxExpansions, prefixLength, transpositions, fuzziness);
            return this;
        }

        public IESQuery<TEntity> Term(Expression<Func<TEntity, object>> expression, object value)
        {
            Builder.Term(expression, value);
            return this;
        }

        public IESQuery<TEntity> Terms(Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            Builder.Terms(expression, values);
            return this;
        }

        public IESQuery<TEntity> Range(Expression<Func<TEntity, object>> expression, object minValue, object maxValue, Boundary boundary = Boundary.Neither)
        {
            Builder.Range(expression, minValue,maxValue,boundary);
            return this;
        }

        public IESQuery<TEntity> Aggs(Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> aggsFunc)
        {
            Builder.Aggs(aggsFunc);
            return this;
        }
    }
}
