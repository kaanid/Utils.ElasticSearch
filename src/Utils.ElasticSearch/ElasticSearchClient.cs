using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elasticsearch.Net;
using Nest;
#if NETSTANDARD2_0
using Microsoft.Extensions.Options;
#endif

namespace Utils.ElasticSearch
{
    public class ElasticSearchClient : IElasticSearchClient
    {
        private readonly ElasticSearchOption _option;
        private readonly IConnectionSettingsValues settingsValues;
        private ElasticClient _client = null;
        private object _objLock = new object();
        public ElasticSearchClient(string url):this(new string[] { url})
        {

        }

#if NETSTANDARD2_0
        public ElasticSearchClient(IOptions<ElasticSearchOption> options) : this(options.Value)
        {

        }
#endif

        public ElasticSearchClient(string[] urls):this(new ElasticSearchOption() { Servers=urls})
        {
            if (urls == null || urls.Length == 0)
                throw new ArgumentNullException(nameof(urls));
        }

        public ElasticSearchClient(ElasticSearchOption option)
        {
            if (option == null || option.Servers==null || option.Servers.Length==0)
                throw new ArgumentNullException(nameof(option));

            _option = option;

            if (option.Servers.Length == 1)
            {
                string url = option.Servers[0];
                if (string.IsNullOrWhiteSpace(url))
                    throw new ArgumentNullException(nameof(url));
                var uri = new Uri(url);
                settingsValues = new ConnectionSettings(uri);
                return;
            }

            IConnectionPool pool = new StaticConnectionPool(option.Servers.Select(m=>new Uri(m)));
            settingsValues = new ConnectionSettings(pool);
        }

        public ElasticSearchClient(IConnectionPool pool)
        {
            if (pool==null)
                throw new ArgumentNullException(nameof(pool));

            settingsValues = new ConnectionSettings(pool);
        }

        public IElasticClient GetClient()
        {
            if(_client!=null)
            {
                return _client;
            }

            lock (_objLock)
            {
                if (_client == null)
                {
                    _client = new ElasticClient(settingsValues);
                }
            }

            if(_client==null)
                throw new ArgumentNullException(nameof(_client));

            return _client;
        }
    }
}
