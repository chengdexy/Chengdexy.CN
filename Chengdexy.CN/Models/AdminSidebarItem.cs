﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.Models
{
    public class AdminSidebarItem
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}