using Chengdexy.CN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.ViewModels
{
    public class AboutItemVM
    {
        public List<AboutItem> ItemList { get; set; }
        public string ImageTitle { get; set; }
        public string ImageUrl { get; set; }

        public AboutItemVM(string title, string url, List<AboutItem> list)
        {
            ImageTitle = title;
            ImageUrl = url;
            ItemList = list;
        }
    }
}