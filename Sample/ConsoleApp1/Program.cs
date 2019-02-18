using Utils.ElasticSearch;
using Utils.ElasticSearch.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //BookTest();
            //InsertWeibo2();
            InsertWeibo4();

            var conf = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                 .Build();

            var sc = new ServiceCollection()
                .AddLogging(logger =>
                {
                    logger
                    .AddConfiguration(conf.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
                })
                .AddSingleton<IConfiguration>((s) => conf)
                .AddElasticSearch(new string[] { "http://localhost:9200" });
            //.AddElasticSearch(new string[] { "http://10.25.57.66:9200" });

            var sp = sc.BuildServiceProvider();

            var query = sp.GetService<IESQuery<Weibo>>();

            query
                .Where(m => m.Title.Contains("asad"))
                .Wildcard(f=>f.Title,"asdf")
                //.WhereIf(m => m.Title == "1111", false)
                //.Sort(f => f.Title, false);
                .Page(2, 11)
                .Type("Weibo");

            var json9 = query.GetJson();
            Console.WriteLine(json9);
            var list9 = query.ToList();

            query.Where(m => m.Title.Contains("asad"));
            var json6 = query.GetJson();
            Console.WriteLine(json6);

            var papernameList = new List<string>() { "股票入门", "美股滚动", "信息披露", "公司公告", "基金公告", "定期报告", "股城网", "分类信息", "无忧考网", "证劵之星", "汉丰网" };
            query.Where(m => papernameList.Contains(m.Title));
            var json4 = query.GetJson();
            Console.WriteLine(json4);

            query.Range(f => f.Createtime, DateTime.Now.AddMonths(-10), DateTime.Now,Utils.Util.Datas.Queries.Boundary.Both);
            var json2 = query.GetJson();
            Console.WriteLine(json2);
            var list2=query.ToList();
            Console.WriteLine($"query.ToList() {list2?.Count}");

            

            //query
            //    .Index("weibo")
            //    .Where(q => q.ArticleSequenceId > 0)
            //    .Aggs(f => f.Max("max_contentwordscount", mbad => mbad.Field("contentwordscount")));


            //query
            //    .Index("weibo")
            //    .Where(q => q.ArticleSequenceId > 0)
            //    .Aggs(f => f.Stats("max_contentwordscount", mbad => mbad.Field("contentwordscount")));

            //query
            //    .Index("weibo")
            //    .Where(q => q.ArticleSequenceId > 0)
            //    .Aggs(f => f.TopHits("max_contentwordscount_hit", mbad => mbad.Sort(f2=>f2.Ascending(m=>m.Contentwordscount))));

            //query
            //    .Index("weibo")
            //    .Where(q => q.ArticleSequenceId > 0)
            //    .Aggs(f => f.ValueCount("max_contentwordscount_hit", mbad => mbad.Field("contentwordscount")));

            //query
            //    .Index("weibo")
            //    .Where(q => q.ArticleSequenceId > 0)
            //    .Aggs(f => f.Cardinality("max_contentwordscount_cardinality", mbad => mbad.Field("contentwordscount")));

            query
                .Index("weibo")
                .Where(q => q.ArticleSequenceId > 0)
                .Aggs(f => f.Terms("max_contentwordscount_terms", mbad => mbad.Field("contentwordscount").Order(f2=>f2.CountAscending())));



            var json = query.GetJson();
            var queryList = query.ToDict();


            Console.Read();
        }

        static void BookTest()
        {
            var conf = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                 .Build();

            var sc = new ServiceCollection()
                .AddLogging(logger =>
                {
                    logger
                    .AddConfiguration(conf.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
                })
                .AddSingleton<IConfiguration>((s) => conf)
                .AddElasticSearch(conf);
                //.AddElasticSearch(new string[] { "http://localhost:9200" });

            var sp = sc.BuildServiceProvider();

            IESQuery<Book> query = null;

            query = sp.GetService<IESQuery<Book>>();

            query.Clear();
            var queryToPagerList = query.Where(book => book.Id == 1).Skip(0).Take(100);
            string str = queryToPagerList.GetJson();
            var result = queryToPagerList.ToPagerList();
            int total = result.TotalCount;
            Console.WriteLine($"query.Where(book => book.Id == 1).ToPagerList() total:{total}");

            query.Clear();
            var query2 = query.Where(book => book.Id == 1);
            string str2 = query2.GetJson();
            var list = query2.ToList();
            int count = list.Count;
            Console.WriteLine($"query.Where(book => book.Id == 1).ToList() count:{count}");

            //ESQueryFactory.Init("http://localhost:9200");
            //query = ESQueryFactory.Create<Book>();

            query.Clear();
            var arr = new List<long> { 1, 2, 3, 4, 5, 6 };
            var q2 = query.Where(b => arr.Contains(b.Id));
            var json2 = q2.GetJson();
            var arrayList = q2.ToList();
            Console.WriteLine($"query.Where(b => arr.Contains(b.Id)) count:{arrayList.Count}");

            query.Clear();
            var idsList = query.Ids(new string[] { "1", "123123" }).ToList();
            Console.WriteLine($"query.Ids(f => f.Id, new string[] {{ \"1\",\"123123\" }}) ids:{idsList.Count}");

            

            query.Clear();
            list = query.MatchAll().ToList();
            count = list.Count;
            Console.WriteLine($"query.MatchAll().ToList() count:{count}");

            query.Clear();
            list = query.Where(book => book.Title == "").ToList();
            count = list.Count;
            Console.WriteLine($"query.Where(book=>book.Title==\"\").ToList() count:{count}");

            list = query.TextKeyword(book => book.Title, "").ToList();
            count = list.Count;
            Console.WriteLine($"query.TextKeyword(book => book.Title, \"\") count:{count}");

            query.Clear();
            list = query.Where(book => book.Title != null).ToList();
            count = list.Count;
            Console.WriteLine($"query.Where(book => book.Title !=null) count:{count}");

            query.Clear();
            list = query.Where(book => book.Title != "abd").ToList();
            count = list.Count;
            Console.WriteLine($"query.Where(book => book.Title != \"abd\") count:{count}");

            query.Clear();
            list = query.Where(book => book.Created > DateTime.Now.AddDays(-1)).ToList();
            count = list.Count;
            Console.WriteLine($"book => book.Created >DateTime.Now.AddDays(-1)).ToList() count:{count}");

            query.Clear();
            list = query.Where(book => book.Created > DateTime.Now.AddDays(-1) || book.Pages > 10 && book.Pages < 10000).ToList();
            count = list.Count;
            Console.WriteLine($"query.Where(book => book.Created > DateTime.Now.AddDays(-1) || book.Pages>10 && book.Pages<10000).ToList() count:{count}");
        }

        static void InsertWeibo()
        {
            //http://10.25.57.66:9200/fwsmtnew_weibo2018q4/_search?size=100
            var client = new ElasticSearchClient("http://localhost:9200");
            var sclient = client.GetClient();

            sclient.CreateIndex("weibo",f=>f.Mappings(m=>m.Map<Weibo>(map=>map
                    .Properties(p=>p
                        .Keyword(k=>k.Name(n=>n.ArticleId))
                        .Keyword(k => k.Name(n => n.Url))
                        .Keyword(k => k.Name(n => n.Editor))
                        .Keyword(k => k.Name(n => n.Class1))
                        .Keyword(k => k.Name(n => n.Class2))
                        .Text(k=>k.Name(n=>n.ContentTxt).Fields(fs=>fs.Keyword(ss=>ss.Name("keywork"))))
                        .Text(k => k.Name(n => n.Title))
                        .Number(k=>k.Name(n=>n.ArticleSequenceId).Type(Nest.NumberType.Long))
                        .Number(k => k.Name(n => n.Contentwordscount).Type(Nest.NumberType.Integer))
                        .Date(k=>k.Name(n=>n.CreateIndex_Time))
                        .Date(k => k.Name(n => n.Createtime))
                        )
                    )
                ));

            var httpClient = new HttpClient();
            for (int i = 0; i < 10; i++)
            {
                string url = $"http://10.25.57.66:9200/fwsmtnew_weibo2018q4/_search?size=100&from={i * 100}";
                var strjson = httpClient.GetStringAsync(url).Result;
                if (strjson != null)
                {
                    var obj = JsonConvert.DeserializeObject<dynamic>(strjson);
                    foreach (var hit in obj.hits.hits)
                    {
                        Weibo wb = JsonConvert.DeserializeObject<Weibo>(hit._source.ToString());
                        var result=sclient.Create<Weibo>(wb, cd => cd.Index("weibo").Id(wb.ArticleSequenceId));
                    }
                }
            }
        }

        static void InsertWeibo2()
        {
            ESFactory.Add("http://localhost:9200");
            var store = ESFactory.CreateStore<Weibo>();

            Weibo weibo = CreateWeibo("");
            store.Add(weibo, "weibo", f=>f.ArticleSequenceId);

            List<Weibo> list = new List<Weibo>();
            foreach(var i in Enumerable.Range(0,10))
            {
                list.Add(CreateWeibo("list-"+i));
            }

            
            store.Add(list, "weibo", f => f.ArticleSequenceId);

            //store.Remove(weibo, "weibo");
            //store.Remove(weibo.ArticleSequenceId, "weibo", "weibo");
            var list2 = new object[] { weibo.ArticleSequenceId };
            store.Remove(list2, "weibo");
            //store.Remove(list);

            var query = ESFactory.Create<Weibo>();

            var list4=query
                .Index("weibo")
                .Ids(new string[] { list.First().ArticleSequenceId.ToString() })
                .ToList();


            if(list.Count>0)
            {
                var model = list.First();
                model.Class1 = "AAAAAAAAAAA2";
                model.Class2 = "bbbbbbbbbbbb2";


                //store.Update(model, f => f.ArticleSequenceId, "weibo");
                store.Update(new Weibo[] { model }, f => f.ArticleSequenceId, "weibo");
            }


            Console.WriteLine("InsertWeibo2");
        }

        static void InsertWeibo4()
        {
            ESFactory.Add("http://localhost:9200");
            var store = ESFactory.CreateStore<Weibo>();
            store.Add(CreateWeibo("InsertWeibo4"), "weibo");

            Console.WriteLine("InsertWeibo4");
        }
        static Weibo CreateWeibo(string title)
        {
            Weibo weibo = new Weibo
            {
                ArticleId = DateTime.Now.Ticks.ToString(),
                ArticleSequenceId = DateTime.Now.Ticks,
                ContentTxt = $"hello {title} {DateTime.Now}",
                Class1 = DateTime.Now.Date.ToString(),
                Createtime = DateTime.Now,
                Title = $"{title}-" + DateTime.Now.Second
            };
            return weibo;
        }
    }
}
