using Chengdexy.CN.DAL;
using Chengdexy.CN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class ShowController : Controller
    {
        private MainContext db = new MainContext();

        // GET: Show
        public ActionResult Index(int ID = 1)
        {
            var blogPage = db.BlogPages.ToList().Where(bp => (bp.ID == ID)).FirstOrDefault();
            if (blogPage==null)
            {
                blogPage = new BlogPage();
            }
            return View(blogPage);
        }
    }
}