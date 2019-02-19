using System;
using System.Collections.Generic;
using System.Text;
using Utils.ElasticSearch.Queries;
using Utils.ElasticSearch.Queries.Abstractions;
using Utils.ElasticSearch.Queries.Core;

#if NETSTANDARD2_0
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Utils.ElasticSearch
{
    public static class Extensions
    {
#if NETSTANDARD2_0
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, string[] urls)
        {
            services.AddSingleton<IElasticSearchClient>(new ElasticSearchClient(urls));
            services.AddScoped(typeof(IESBuilder<>), typeof(ESBuilderBase<>));
            services.AddScoped(typeof(IESQuery<>), typeof(ESQuery<>));
            services.AddScoped(typeof(IESStores<>), typeof(ESStoreBase<>));

            return services;
        }

        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration conf)
        {
            services.AddSingleton<IElasticSearchClient, ElasticSearchClient>();
            services.AddScoped(typeof(IESBuilder<>), typeof(ESBuilderBase<>));
            services.AddScoped(typeof(IESQuery<>), typeof(ESQuery<>));
            services.AddScoped(typeof(IESStores<>), typeof(ESStoreBase<>));
            services.Configure<ElasticSearchOption>(conf.GetSection("ElasticSearch"));

            return services;
        }
#endif
    }
}


