using Chengdexy.CN.DAL;
using Chengdexy.CN.Models;
using Chengdexy.CN.Utils;
using Chengdexy.CN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class AdminController : Controller
    {
        private MainContext db = new MainContext();

        //
        // GET: Admin

        public ActionResult Index()
        {
            if (!(Session.Count > 0) || !((bool)Session["AdminLoggedIn"] == true))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.JumbotronCount = db.Jumbotrons.Count();
            ViewBag.ProgramCount = db.Programs.Count();
            ViewBag.EditionCount = db.ProgramEditions.Count();
            ViewBag.BlogCount = db.BlogPages.Count();
            return View();
        }

        //
        // GET: Admin/AdminSettings

        public ActionResult AdminSettings()
        {
            if (!(Session.Count > 0) || !((bool)Session["AdminLoggedIn"] == true))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // POST: Admin/AdminSettings

        [HttpPost]
        public ActionResult AdminSettings(FormCollection fc)
        {
            string newAccount = fc["newAccount"];
            string newPassword = fc["newPassword"];
            string newPasswordAgain = fc["newPasswordAgain"];
            if (string.IsNullOrEmpty(newAccount)|string.IsNullOrEmpty(newPassword )|string.IsNullOrEmpty(newPasswordAgain ))
            {
                //输入项为空
                return View();
            }
            if (newPassword!=newPasswordAgain)
            {
                //两次输入不一致
                return View();
            }
            string oldAccount = fc["inputAccount"];
            string oldPassword = fc["inputPassword"];
            oldPassword = MD5maker.GetMd5Hash(MD5.Create(), oldPassword);
            AdminAccount aa = db.AdminAccounts.FirstOrDefault();
            if (oldAccount!=aa.Account || oldPassword!=aa.Password )
            {
                //帐号或密码错误
                return View();
            }
            aa.Account = newAccount;
            aa.Password = MD5maker.GetMd5Hash(MD5.Create(), newPassword);
            db.Entry(aa).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult ShowSidebar()
        {
            var itemList = db.AdminSidebarItems.ToList();

            return PartialView("~/Views/Shared/_PartialAdminSidebar.cshtml", new SidebarItemVM(itemList, 1));
        }
    }
}