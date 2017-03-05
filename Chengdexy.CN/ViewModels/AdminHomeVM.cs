using Chengdexy.CN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.ViewModels
{
    public class AdminHomeVM
    {
        public string adminName { get; set; }
        public int blogCount { get; set; }
        public int programCount { get; set; }
        public int editionCount { get; set; }
        public List<Program> programList { get; set; }
    }
}