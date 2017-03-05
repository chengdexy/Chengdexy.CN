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
    public class HomeController : Controller
    {
        private MainContext db = new MainContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ShowJumbotron()
        {
            var jumbotron = db.Jumbotrons.OrderByDescending(p => p.ID).FirstOrDefault();

            return PartialView("~/Views/Shared/_PartialJumbotron.cshtml", jumbotron);
        }

        [ChildActionOnly]
        public ActionResult ShowSketchList()
        {
            var sketchList = db.BlogPages.Select(sl => new BlogSketchVM
            {
                BlogPageId = sl.ID,
                BlogPageTitle = sl.Title,
                BlogPageSketch = sl.Sketch,
                BlogPageImagePath = sl.ImagePath
            }).OrderByDescending(sl=>sl.BlogPageId) .ToList();
            return PartialView("~/Views/Shared/_PartialSketchList.cshtml", sketchList);
        }

        [ChildActionOnly]
        public ActionResult ShowNavbar()
        {
            var itemList = db.NavbarItems.ToList();

            return PartialView("~/Views/Shared/_PartialNavbar.cshtml", new NavbarItemVM(itemList, 1));
        }
    }
}