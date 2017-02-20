using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.Models
{
    public class AboutItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public AboutItem(string text, string value)
        {
            Text = text;
            Value = value;
        }
    }
}