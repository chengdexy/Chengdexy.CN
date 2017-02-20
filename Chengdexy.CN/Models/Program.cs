using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.Models
{
    public class Program
    {
        public int ID { get; set; }
        public string Ename { get; set; }
        public string Cname { get; set; }
        public string Motive { get; set; }
        public string Describe { get; set; }
        public List<ProgramEdition> ProgramEditions { get; set; }
    }
}