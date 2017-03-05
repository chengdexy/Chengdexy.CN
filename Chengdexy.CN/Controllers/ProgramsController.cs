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
    public class ProgramsController : Controller
    {
        private MainContext db = new MainContext();

        // GET: Programs
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ShowProgramList()
        {
            var programList = db.Programs.OrderByDescending(pl => pl.ProgramEditions.Max(pe => pe.PublishDate)).ToList();
            return PartialView("~/Views/Shared/_PartialProgramList.cshtml", programList);
        }

        [ChildActionOnly]
        public ActionResult ShowNavbar()
        {
            var itemList = db.NavbarItems.ToList();

            return PartialView("~/Views/Shared/_PartialNavbar.cshtml", new NavbarItemVM(itemList, 2));
        }
    }
}