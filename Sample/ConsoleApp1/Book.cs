using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Book
    {
        public long Id { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
        public int Pages { set; get; }
        public DateTime Created { set; get; }
    }
}
