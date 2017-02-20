using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.Models
{
    /// <summary>
    /// 描述: 用于首页的,博文简述部件
    /// 应用于: _PartialSketchList
    /// 传递时: 传递Sketch对象的列表
    /// </summary>
    public class BlogSketch
    {
        public int ID { get; set; }
        public string ImageFileName { get; set; }
        public string BlogCapital { get; set; }
        public string BlogDescribe { get; set; }
        public string BlogUrl { get; set; }
    }
}