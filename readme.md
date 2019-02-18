# ElasticSearch

## 支持

- net461
- dotnet 2.2

## 使用


#### net461

```
        ESQueryFactory.Init("http://localhost:9200");
        var query = ESQueryFactory.Create<Book>();

        var list = query.Where(book => book.Id == 1).ToList();
        int count = list.Count;

        Console.WriteLine($"query.Where(book => book.Id == 1).ToList() count:{count}");

        query.Clear();
        list = query.MatchAll().ToList();
        count = list.Count;
        Console.WriteLine($"query.MatchAll().ToList() count:{count}");

        query.Clear();
        list = query.Where(book => book.Title == "").ToList();
        count = list.Count;
        Console.WriteLine($"query.Where(book=>book.Title==\"\").ToList() count:{count}");

        query.Clear();
        list = query.Where(book => book.Created > DateTime.Now.AddDays(-1)).ToList();
        count = list.Count;
        Console.WriteLine($"query.Where(book => book.Created >DateTime.Now.AddDays(-1)).ToList() count:{count}");


```

#### dotnet

```

        var sc = new ServiceCollection()
            .AddElasticSearch(conf);
            //.AddElasticSearch(new string[] { "http://localhost:9200" });

        var sp = sc.BuildServiceProvider();

        IESQuery<Book> query = null;

        query = sp.GetService<IESQuery<Book>>();

        var arr= new List<long>{ 1, 2, 3, 4, 5, 6 };
        var q2 = query.Where(b => arr.Contains(b.Id));
        var json2= q2.GetJson();
        var arrayList = q2.ToList();
        Console.WriteLine($"query.Where(b => arr.Contains(b.Id)) count:{arrayList.Count}");

```