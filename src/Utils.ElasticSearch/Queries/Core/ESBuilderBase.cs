using Elasticsearch.Net;
using Utils.ElasticSearch.Queries.Abstractions;
using Utils.ElasticSearch.Queries.Clauses;
using Utils.Util.Datas.Queries;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Utils.ElasticSearch.Queries.Core
{
    public class ESBuilderBase<TEntity> : IESBuilder<TEntity> where TEntity : class
    {
        /// <summary>
        /// 实体解析器
        /// </summary>
        protected IEntityResolver EntityResolver { get; }
        private readonly QueryContainerDescriptor<TEntity> _queryContainerDescriptor = null;
        private ISearchRequest _searchRequest = null;

        public ESBuilderBase()
        {
            EntityResolver = new EntityResolver();
            _queryContainerDescriptor = new QueryContainerDescriptor<TEntity>();
        }

        public void Clear()
        {
            _fieldClause = null;
            _indexClause = null;
            _skipClause = null;
            _sortClause = null;
            _whereClause = null;
            _takeClause = null;
            _typeClause = null;
            _searchRequest = null;
        }
        
        public IESBuilder<TEntity> New()
        {
            return new ESBuilderBase<TEntity>();
        }

        #region FieldClause

        private IFieldClause _fieldClause;
        protected IFieldClause FieldClause
        {
            get
            {
                if (_fieldClause == null)
                    _fieldClause = new FieldClause(EntityResolver);
                return _fieldClause;
            }
        }

        public IESBuilder<TEntity> Field(string columns)
        {
            FieldClause.Field(columns);
            return this;
        }

        public IESBuilder<TEntity> Field(Expression<Func<TEntity, object[]>> expression)
        {
            FieldClause.Field(expression);
            return this;
        }

        public IESBuilder<TEntity> Field(Expression<Func<TEntity, object>> expression)
        {
            FieldClause.Field(expression);
            return this;
        }

        public IEnumerable<string> GetField()
        {
            return FieldClause.GetColumns();
        }
        #endregion

        #region IndexClause
        private IIndexClause _indexClause;
        protected IIndexClause IndexClause => _indexClause != null ? _indexClause : (_indexClause = new IndexClause());

        public IESBuilder<TEntity> Index(string table)
        {
            IndexClause.Index(table);
            return this;
        }

        public IESBuilder<TEntity> Index()
        {
            IndexClause.Index<TEntity>();
            return this;
        }

        public IEnumerable<string> GetIndexs()
        {
            var list = IndexClause.GetIndexs();
            if (list == null || !list.Any())
                return new string[] { typeof(TEntity).Name.ToLower() };
            return IndexClause.GetIndexs();
        }
        #endregion

        #region Type
        private ITypeClause _typeClause;
        protected ITypeClause typeClause => _typeClause != null ? _typeClause : (_typeClause = new TypeClause());

        public IESBuilder<TEntity> Type(string type)
        {
            typeClause.Type(type);
            return this;
        }

        public string GetIndexType()
        {
            return typeClause.IndexType;
        }
        #endregion

        #region SkipClause
        private ISkipClause _skipClause;
        protected ISkipClause SkipClause => _skipClause != null ? _skipClause : (_skipClause = new SkipClause());

        public IESBuilder<TEntity> Skip(int index)
        {
            SkipClause.Skip(index);
            return this;
        }

        public int GetSkip()
        {
            return SkipClause.Index;
        }

        #endregion

        #region SortClause
        private ISortClause<TEntity> _sortClause;
        protected ISortClause<TEntity> SortClause => _sortClause != null ? _sortClause : (_sortClause = new SortClause<TEntity>());
        public IESBuilder<TEntity> SortAsc(Expression<Func<TEntity, object>> expression)
        {
            SortClause.SortAsc(expression);
            return this;
        }

        public IESBuilder<TEntity> SortAsc(string field)
        {
            SortClause.SortAsc(field);
            return this;
        }

        public IESBuilder<TEntity> SortDesc(Expression<Func<TEntity, object>> expression)
        {
            SortClause.SortDesc(expression);
            return this;
        }

        public IESBuilder<TEntity> SortDesc(string field)
        {
            SortClause.SortDesc(field);
            return this;
        }
        public IESBuilder<TEntity> Sort(Expression<Func<TEntity, object>> expression, bool isAsc)
        {
            SortClause.Sort(expression,isAsc);
            return this;
        }
        public SortDescriptor<TEntity> GetSort()
        {
            return SortClause.GetSort();
        }
        #endregion

        #region TakeClause
        private ITakeClause _takeClause;
        protected ITakeClause TakeClause => _takeClause != null ? _takeClause : (_takeClause = new TakeClause());
        public IESBuilder<TEntity> Take(int count)
        {
            TakeClause.Take(count);
            return this;
        }
        public int GetTake()
        {
            return TakeClause.Count;
        }
        #endregion

        #region PageSetting
        public IESBuilder<TEntity> Page(int pageIndex,int pageSize=10)
        {
            if (pageIndex < 1)
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "pageIndex must > 0");

            if(pageSize<1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "pageIndex must > 0");

            Skip((pageIndex - 1) * pageSize);
            Take(pageSize);

            return this;
        }
        #endregion

        #region WhereClause
        private IWhereClause<TEntity> _whereClause;
        protected IWhereClause<TEntity> WhereClause => _whereClause != null ? _whereClause : (_whereClause = new WhereClause<TEntity>(EntityResolver, _queryContainerDescriptor));

        public IESBuilder<TEntity> Ids(string[] values)
        {
            WhereClause.Ids(values);
            return this;
        }
        public IESBuilder<TEntity> Match(Expression<Func<TEntity, object>> expression, string value)
        {
            WhereClause.Match(expression, value);
            return this;
        }
        public IESBuilder<TEntity> MatchIf(Expression<Func<TEntity, object>> expression, string value, bool condition)
        {
            WhereClause.MatchIf(expression, value,condition);
            return this;
        }
        public IESBuilder<TEntity> MatchAll()
        {
            WhereClause.MatchAll();
            return this;
        }
        public IESBuilder<TEntity> Must(Abstractions.ICondition condition)
        {
            WhereClause.Must(condition);
            return this;
        }
        public IESBuilder<TEntity> MustNot(Abstractions.ICondition condition)
        {
            WhereClause.MustNot(condition);
            return this;
        }
        public IESBuilder<TEntity> Should(Abstractions.ICondition condition)
        {
            WhereClause.Should(condition);
            return this;
        }
        public IESBuilder<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            WhereClause.Where(expression);
            return this;
        }
        public IESBuilder<TEntity> WhereIf(Expression<Func<TEntity, bool>> expression, bool condition)
        {
            WhereClause.WhereIf(expression,condition);
            return this;
        }

        public QueryContainer GetWhere()
        {
            return WhereClause.GetContainer();
        }
        public IESBuilder<TEntity> Wildcard(Expression<Func<TEntity, object>> expression, string value)
        {
            WhereClause.Wildcard(expression, value);
            return this;
        }

        public IESBuilder<TEntity> Regexp(Expression<Func<TEntity, object>> expression, string value)
        {
            WhereClause.Regexp(expression, value);
            return this;
        }

        public IESBuilder<TEntity> TextKeyword(Expression<Func<TEntity, object>> expression, string value)
        {
            WhereClause.TextKeyword(expression, value);
            return this;
        }

        public IESBuilder<TEntity> Fuzzy(Expression<Func<TEntity, object>> expression, string value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false)
        {
            WhereClause.Fuzzy(expression, value, maxExpansions, prefixLength, transpositions);
            return this;
        }

        public IESBuilder<TEntity> FuzzyNumeric(Expression<Func<TEntity, object>> expression, double value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, double fuzziness = 2)
        {
            WhereClause.FuzzyNumeric(expression, value, maxExpansions, prefixLength, transpositions, fuzziness);
            return this;
        }
        public IESBuilder<TEntity> FuzzyDate(Expression<Func<TEntity, object>> expression, DateTime value, int maxExpansions = 100, int prefixLength = 2, bool transpositions = false, TimeSpan? fuzziness = null)
        {
            WhereClause.FuzzyDate(expression, value, maxExpansions, prefixLength, transpositions, fuzziness);
            return this;
        }

        public IESBuilder<TEntity> Term(Expression<Func<TEntity, object>> expression, object value)
        {
            WhereClause.Term(expression, value);
            return this;
        }

        public IESBuilder<TEntity> Terms(Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            WhereClause.Terms(expression, values);
            return this;
        }

        public IESBuilder<TEntity> Range(Expression<Func<TEntity, object>> expression, object minValue, object maxValue, Boundary boundary = Boundary.Neither)
        {
            WhereClause.Range(expression, minValue,maxValue,boundary);
            return this;
        }

        #endregion

        #region aggregation
        private IAggregationClause<TEntity> _aggregationClause;
        protected IAggregationClause<TEntity> AggregationClause => _aggregationClause != null ? _aggregationClause : (_aggregationClause = new AggregationClause<TEntity>());

        public IESBuilder<TEntity> Aggs(Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> aggregationsSelector)
        {
            AggregationClause.Aggs(aggregationsSelector);
            return this;
        }
        public Func<AggregationContainerDescriptor<TEntity>, IAggregationContainer> GetAggs()
        {
            return AggregationClause.GetAggs();
        }

        #endregion

        #region ToObject
        public string ToJson()
        {
            var sd = ToRequest();
            var connection = new InMemoryConnection();
            var settings = new ConnectionSettings(connection);
            var client = new ElasticClient(settings);
            return client.Serializer.SerializeToString(sd, SerializationFormatting.Indented);
        }

        public Func<SearchDescriptor<TEntity>, ISearchRequest> ToFunc()
        {
            Func<SearchDescriptor<TEntity>, ISearchRequest> func = null;

            func = searchDescriptor =>
            {
                searchDescriptor
                .Query(q =>
                {
                    return GetWhere();
                })
                .Index(GetIndexs().ToArray())
                .Sort(sortd => GetSort())
                .From(GetSkip())
                .Size(GetTake());

                var list = GetField();
                if (list!=null)
                {
                    searchDescriptor.StoredFields(f => f.Fields(list.Select(m => (Field)m)));
                }

                var indexType = GetIndexType();
                if (indexType != null)
                    searchDescriptor.Type(indexType);

                var aggs = GetAggs();
                if(aggs!=null)
                {
                    searchDescriptor.Aggregations(aggs)
                    .Size(0);
                }

                return searchDescriptor;
            }; 

            return func;
        }

        public ISearchRequest ToRequest()
        {
            if (_searchRequest == null)
            {
                var sd = new SearchDescriptor<TEntity>();
                var func = ToFunc();
                _searchRequest = func?.Invoke(sd) ?? sd;
            }
            return _searchRequest;
        }
        #endregion
    }
}
