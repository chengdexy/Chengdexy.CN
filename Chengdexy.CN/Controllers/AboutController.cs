using Chengdexy.CN.DAL;
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
        private MainContext db = new MainContext();

        // GET: About
        public ActionResult Index()
        {
            string imgTitle = "Now you know what's wild-programmer :P";
            string imgUrl = "/Images/itsme.jpg";
            var list = db.AboutItems.ToList();
            AboutItemVM aivm = new ViewModels.AboutItemVM(imgTitle, imgUrl, list );
            return View(aivm);
        }

        [ChildActionOnly]
        public ActionResult ShowNavbar()
        {
            var itemList = db.NavbarItems.ToList();

            return PartialView("~/Views/Shared/_PartialNavbar.cshtml", new NavbarItemVM(itemList, 4));
        }
    }
}