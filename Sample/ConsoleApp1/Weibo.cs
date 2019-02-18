using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Weibo
    {
        public string ArticleId { set; get; }
        public long ArticleSequenceId { set; get; }
        public string Class1 { set; get; }
        public string Class2 { set; get; }
        public string ContentTxt { set; get; }

        public int Contentwordscount { set; get; }

        public DateTime CreateIndex_Time { set; get; }
        public DateTime Createtime { set; get; }

        public string Editor { set; get; }

        public string Title { set; get; }

        public string Url { set; get;}
    }
}
