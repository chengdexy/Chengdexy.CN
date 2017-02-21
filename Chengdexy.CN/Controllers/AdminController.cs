using Chengdexy.CN.DAL;
using Chengdexy.CN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class AdminController : Controller
    {
        private MainContext db = new MainContext();

        // GET: Admin
        public ActionResult Index()
        {
            if (!(Session.Count>0) || !((bool)Session["AdminLoggedIn"] == true))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult ShowSidebar()
        {
            var itemList = db.AdminSidebarItems.ToList();

            return PartialView("~/Views/Shared/_PartialAdminSidebar.cshtml", new SidebarItemVM(itemList, 1));
        }
    }
}