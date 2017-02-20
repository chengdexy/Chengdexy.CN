using Chengdexy.CN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.ViewModels
{
    public class NavbarItemVM
    {
        public int ActiveIndex { get; set; }
        public List<NavbarItem> NavbarItems { get; set; }

        public NavbarItemVM(List<NavbarItem> list, int index)
        {
            ActiveIndex = index;
            NavbarItems = list;
        }
    }
}