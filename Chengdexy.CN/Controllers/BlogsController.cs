using Chengdexy.CN.Models;
using Chengdexy.CN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class BlogsController : Controller
    {
        // GET: Blogs
        public ActionResult Index()
        {
            return View();
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
                Text = "Blogs",
                Route = "Blogs",
                Action = "Index"
            };
            itemList.Add(nav);

            return PartialView("~/Views/Shared/_PartialNavbar.cshtml", new NavbarItemVM(itemList, 2));
        }
    }
}