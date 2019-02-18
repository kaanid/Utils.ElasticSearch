using Utils.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppNet461
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ESFactory.Add("http://localhost:9200");
            var query = ESFactory.Create<Book>();

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
        }
    }
}
