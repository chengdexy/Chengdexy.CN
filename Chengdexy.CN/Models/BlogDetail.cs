using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.Models
{
    public class BlogPage
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public string Sketch { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
    }
}