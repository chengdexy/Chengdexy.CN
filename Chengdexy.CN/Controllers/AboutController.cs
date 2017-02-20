using Chengdexy.CN.Models;
using Chengdexy.CN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
            string imgTitle = "Now you know what's wild-programmer :P";
            string imgUrl = "img/itsme.jpg";
            List<AboutItem> list = new List<Models.AboutItem>();
            list.Add(new Models.AboutItem("Name", "Chengdexy"));
            list.Add(new Models.AboutItem("Sex", "Man"));
            AboutItemVM aivm = new ViewModels.AboutItemVM(imgTitle, imgUrl, list);
            return View(aivm);
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

            return PartialView("~/Views/Shared/_PartialNavbar.cshtml", new NavbarItemVM(itemList, 2));
        }
    }
}