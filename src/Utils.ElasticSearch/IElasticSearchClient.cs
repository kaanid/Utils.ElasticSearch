using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ElasticSearch
{
    public interface IElasticSearchClient
    {
        IElasticClient GetClient();
    }
}
