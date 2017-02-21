using Chengdexy.CN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.ViewModels
{
    public class SidebarItemVM
    {
        public int ActiveIndex { get; set; }
        public List<AdminSidebarItem> SidebarItems { get; set; }

        public SidebarItemVM(List<AdminSidebarItem> list, int index)
        {
            ActiveIndex = index;
            SidebarItems = list;
        }
    }
}