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
    public class BlogsController : Controller
    {
        private MainContext db = new MainContext();

        // GET: Blogs
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ShowNavbar()
        {
            var itemList = db.NavbarItems.ToList();

            return PartialView("~/Views/Shared/_PartialNavbar.cshtml", new NavbarItemVM(itemList, 3));
        }
    }
}