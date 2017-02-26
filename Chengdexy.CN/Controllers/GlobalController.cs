using Chengdexy.CN.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class GlobalController : Controller
    {
        private MainContext db = new MainContext();

        // GET: Global
        public ActionResult Index()
        {
            return View();
            //return RedirectToAction("Index", "Home");
        }

        // GET: Global/SetSiteTitle
        [ChildActionOnly]
        public string SetSiteTitle()
        {
            string title = db.MainSites.FirstOrDefault().TitleText;
            return title;
        }

        // GET: Global/SetSiteMainCapital
        [ChildActionOnly]
        public string SetSiteMainCapital()
        {
            string capital = db.MainSites.FirstOrDefault().MainCapital;
            return capital;
        }

        // GET: Global/SetSiteSubCapital
        [ChildActionOnly]
        public string SetSiteSubCapital()
        {
            string capital = db.MainSites.FirstOrDefault().SubCapital;
            return capital;
        }
    }
}