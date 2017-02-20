using Chengdexy.CN.Models;
using Chengdexy.CN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ShowJumbotron()
        {
            Jumbotron j = new Models.Jumbotron()
            {
                Capital = "LoveSave v1.2发布了!",
                Describe = "开源、免费的QQ情侣导出工具LoveSave v1.2发布了,轻松一键导出QQ情侣中的日志、图片、聊天记录和自定义纪念日,并且生成本地静态网页,随时查看!对了,LoveSave导出记录中,还包括包含所有数据的数据库文件哦!",
                DownloadButtonText = "我要下载!",
                DownloadUrl = "https://github.com/chengdexy/LoveSave/releases/download/v1.2.0.1/LoveSave1201.zip"
            };
            return PartialView("~/Views/Shared/_PartialJumbotron.cshtml", j);
        }

        [ChildActionOnly]
        public ActionResult ShowSketchList()
        {
            List<BlogSketch> list = new List<Models.BlogSketch>();
            BlogSketch bs = new Models.BlogSketch()
            {
                BlogCapital = "MVC+HTML5+Bootstrap3做博客(0)",
                BlogDescribe = "说起来，现在看到的Chengdexy.Cn其实是一个静态站，也就是说这总计3个页面都是静态html页。今天开始，我来一步步把它改造成正经的“网站”！",
                BlogUrl = "~/HTML/mvc_html5_bootstrap_make_site.html",
                ImageFileName = "~/IMG/h5.jpg"
            };
            list.Add(bs);
            bs = new Models.BlogSketch()
            {
                BlogCapital = "用Excel制作米字格练字纸！",
                BlogDescribe = "最近受了刺激就开始练钢笔字了,不过书店卖的米字格纸好贵啊,既然公司有打印机,嗯哼,我自己做吧!",
                BlogUrl = "~/HTML/use_excel_make_mizige.html",
                ImageFileName = "~/IMG/final_complete.jpg"
            };
            list.Add(bs);
            return PartialView("~/Views/Shared/_PartialSketchList.cshtml", list);
        }

        [ChildActionOnly]
        public ActionResult ShowNavbar()
        {
            List<NavbarItem> itemList = new List<NavbarItem>();
            NavbarItem nav = new NavbarItem()
            {
                ID = 1,
                Text = "Home",
                Route = "Home",
                Action = "Index"
            };
            itemList.Add(nav);
            nav = new Models.NavbarItem()
            {
                ID = 2,
                Text = "About",
                Route = "About",
                Action = "Index"
            };
            itemList.Add(nav);

            return PartialView("~/Views/Shared/_PartialNavbar.cshtml", new NavbarItemVM(itemList,1));
        }
    }
}