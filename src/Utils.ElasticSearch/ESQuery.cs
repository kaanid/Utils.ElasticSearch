using Utils.ElasticSearch.Queries;
using Utils.ElasticSearch.Queries.Abstractions;
using Utils.Util.Datas.Repositories;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ElasticSearch
{
    public class ESQuery<TEntity> : ESQueryBase<TEntity> where TEntity : class
    {
        public ESQuery(IESBuilder<TEntity> esBuilder, IElasticSearchClient client=null)
            :base(esBuilder, client)
        {
        }

        public override Nest.ISearchResponse<TEntity> ToResponse()
        {
            return GetResponse();
        }

        public override Task<Nest.ISearchResponse<TEntity>> ToResponseAsync()
        {
            return GetResponseAsync();
        }

        public override TEntity To()
        {
            var list = ToList();
            return list.FirstOrDefault();
        }

        public override async Task<TEntity> ToAsync()
        {
            var list =await ToListAsync();
            return list.FirstOrDefault();
        }

        public override List<TEntity> ToList()
        {
            var res= GetResponse();
            return GetList(res);
        }

        public override async Task<List<TEntity>> ToListAsync()
        {
            var res =await GetResponseAsync();
            return GetList(res);
        }

        public override PagerList<TEntity> ToPagerList()
        {
            var pager = GetPager();

            var res = GetResponse();
            var list= GetList(res);

            pager.TotalCount = (int)res.Total;

            return new PagerList<TEntity>(pager, list);
        }
        public override async Task<PagerList<TEntity>> ToPagerListAsync()
        {
            var pager = GetPager();

            var res = await GetResponseAsync();
            var list= GetList(res);

            pager.TotalCount = (int)res.Total;

            return new PagerList<TEntity>(pager, list);
        }

        public override Dictionary<string, object> ToDict()
        {
            var res = GetResponse();
            return GetDict(res);
        }

        public override async Task<Dictionary<string, object>> ToDictAsync()
        {
            var res = await GetResponseAsync();
            return GetDict(res);
        }

        public override IReadOnlyDictionary<string, IAggregate> ToAggregateDictionary()
        {
            var res = GetResponse();
            return res.Aggregations;
        }

        public override async Task<IReadOnlyDictionary<string, IAggregate>> ToAggregateDictionaryAsync()
        {
            var res = await GetResponseAsync();
            return res.Aggregations;
        }


        #region private
        private async Task<Nest.ISearchResponse<TEntity>> GetResponseAsync()
        {
            var requestData = Builder.ToRequest();
            var client = GetClient();

            var res = await client.SearchAsync<TEntity>(requestData);
            ResponseCheck(res);
            return res;
        }

        private Nest.ISearchResponse<TEntity> GetResponse()
        {
            var requestData = Builder.ToRequest();
            var client = GetClient();

            var res = client.Search<TEntity>(requestData);
            ResponseCheck(res);
            return res;
        }

        private List<TEntity> GetList(Nest.ISearchResponse<TEntity> res)
        {
            return res.Documents.ToList();
        }

        private Dictionary<string, object> GetDict(Nest.ISearchResponse<TEntity> res)
        {
            if (res.Aggregations == null)
                throw new Exception($"ToDict Aggregations {res.ApiCall.HttpMethod} {res.ApiCall.HttpStatusCode}", res.ApiCall.OriginalException);
            Dictionary<string, object> dict = new Dictionary<string, object>();

            int n = res.Aggregations.Count;
            foreach (var m in res.Aggregations)
            {
                dict.Add("Meta", m.Value.Meta);
                switch (m.Value.GetType().Name)
                {
                    case "StatsAggregate":
                        var val2 = m.Value as StatsAggregate;
                        dict.Add("Average", val2.Average);
                        dict.Add("Count", val2.Count);
                        dict.Add("Max", val2.Max);
                        dict.Add("Min", val2.Min);
                        dict.Add("Sum", val2.Sum);
                        break;
                    case "ValueAggregate":
                        var val = m.Value as ValueAggregate;
                        dict.Add("Value", val.Value);
                        break;
                    case "TopHitsAggregate":
                        var val4 = m.Value as TopHitsAggregate;
                        dict.Add("MaxScore", val4.MaxScore);
                        dict.Add("Total", val4.Total);
                        break;
                    case "BucketAggregate":
                        var val6 = m.Value as BucketAggregate;
                        dict.Add("BgCount", val6.BgCount);
                        dict.Add("DocCount", val6.DocCount);
                        dict.Add("DocCountErrorUpperBound", val6.DocCountErrorUpperBound);
                        dict.Add("Items", val6.Items);
                        if (val6.Items != null)
                        {
                            foreach (var m6 in val6.Items)
                            {
                                var v9 = m6 as KeyedBucket<object>;
                                if (v9 != null)
                                    dict.Add($"Items-{v9.Key}", v9.DocCount);
                            }
                        }
                        dict.Add("SumOtherDocCount", val6.SumOtherDocCount);
                        break;
                }
            }
            return dict;
        }

        private void ResponseCheck(Nest.ISearchResponse<TEntity> res)
        {
            if (res == null)
            {
                throw new NullReferenceException("ResponseCheck response is null");
            }

            if (!res.ApiCall.Success)
                throw new Exception($"ResponseCheck ApiCall Success {res.ApiCall.HttpMethod} {res.ApiCall.HttpStatusCode} {res.ServerError.Error.Reason}", res.ApiCall.OriginalException);

            if (!res.IsValid)
                throw new Exception($"ResponseCheck IsValid {res.ApiCall.HttpMethod} {res.ApiCall.HttpStatusCode} {res.ServerError.Error.Reason}", res.ApiCall.OriginalException);

            TryQueryedClear();
        }
        #endregion
    }
}
