using Chengdexy.CN.Models;
using Chengdexy.CN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class ProgramsController : Controller
    {
        // GET: Programs
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ShowProgramList()
        {
            Program p = new Models.Program()
            {
                ID = 1,
                Ename = "LoveSave",
                Cname = "导出工具",
                Motive = "做着玩的",
                Describe = "有很多功能哦"
            };
            ProgramEdition pe = new Models.ProgramEdition()
            {
                ID = 1,
                ProgramID = 1,
                PublishDate = DateTime.Now,
                EditionString = "v1.2.3",
                DownloadUrl = "http://github.com/"
            };
            p.ProgramEditions = new List<ProgramEdition>();
            p.ProgramEditions.Add(pe);
            List<Program> list = new List<Program>();
            list.Add(p);
            list.Add(p);
            return PartialView("~/Views/Shared/_PartialProgramList.cshtml",list);
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
            nav = new Models.NavbarItem()
            {
                ID = 3,
                Text = "Programs",
                Route = "Programs",
                Action = "Index"
            };
            itemList.Add(nav);

            return PartialView("~/Views/Shared/_PartialNavbar.cshtml", new NavbarItemVM(itemList, 3));
        }
    }
}