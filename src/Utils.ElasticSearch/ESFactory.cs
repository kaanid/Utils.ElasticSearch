using Elasticsearch.Net;
using Utils.ElasticSearch.Queries;
using Utils.ElasticSearch.Queries.Core;
using Utils.ElasticSearch.Stores;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Utils.ElasticSearch
{
    public  static class ESFactory
    {
        private static ElasticSearchClient _client=null;
        private static ConcurrentDictionary<string, ElasticSearchClient> _clientList=null;

        public static void Add(string url,string name=null)
        {
            Add(new string[] { url }, name);
        }

        public static void Add(string[] urls,string name)
        {
            if (name == null)
            {
                if (_client != null)
                    throw new Exception("esName:default ES Client exist");

                _client = new ElasticSearchClient(urls);
            }
            else
            {
                if (_clientList == null)
                    _clientList = new ConcurrentDictionary<string, ElasticSearchClient>();

                if (_clientList.Keys.Contains(name))
                {
                    throw new Exception($"esName:{name} ES Client exist");
                }

                _clientList.TryAdd(name, new ElasticSearchClient(urls));
            }
        }

        public static void Add(IConnectionPool pool, string name)
        {
            if (name == null)
            {
                if (_client != null)
                    throw new Exception("esName:default ES Client exist");

                _client = new ElasticSearchClient(pool);
            }
            else
            {
                if (_clientList == null)
                    _clientList = new ConcurrentDictionary<string, ElasticSearchClient>();

                if (_clientList.Keys.Contains(name))
                {
                    throw new Exception($"esName:{name} ES Client exist");
                }

                _clientList.TryAdd(name, new ElasticSearchClient(pool));
            }
        }

        public static IESQuery<TEntity> Create<TEntity>(string name=null) where TEntity:class
        {
            var esBuilder = new ESBuilderBase<TEntity>();

            ElasticSearchClient client = null;
            if(name==null)
            {
                client = _client;
            }
            else
            {
                if (!_clientList.TryGetValue(name, out client))
                    throw new NullReferenceException($"esName:{name} ES Client not exist");
            }

            if(client==null)
                throw new NullReferenceException("ES Client not exist");

            var query = new ESQuery<TEntity>(esBuilder, client);
            return query;
        }

        public static IESStores<TEntity> CreateStore<TEntity>(string name=null) where TEntity : class
        {
            ElasticSearchClient client = null;
            if (name == null)
            {
                client = _client;
            }
            else
            {
                if (!_clientList.TryGetValue(name, out client))
                    throw new NullReferenceException($"esName:{name} ES Client not exist");
            }

            if (client == null)
                throw new NullReferenceException("ES Client not exist");

            var store = new ESStoreBase<TEntity>(client);
            return store;
        }
    }
}
