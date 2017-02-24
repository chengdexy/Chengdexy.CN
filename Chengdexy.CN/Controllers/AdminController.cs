using Chengdexy.CN.DAL;
using Chengdexy.CN.Models;
using Chengdexy.CN.Utils;
using Chengdexy.CN.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (string.IsNullOrEmpty(newAccount) | string.IsNullOrEmpty(newPassword) | string.IsNullOrEmpty(newPasswordAgain))
            {
                //输入项为空
                return View();
            }
            if (newPassword != newPasswordAgain)
            {
                //两次输入不一致
                return View();
            }
            string oldAccount = fc["inputAccount"];
            string oldPassword = fc["inputPassword"];
            oldPassword = MD5maker.GetMd5Hash(MD5.Create(), oldPassword);
            AdminAccount aa = db.AdminAccounts.FirstOrDefault();
            if (oldAccount != aa.Account || oldPassword != aa.Password)
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

        //
        // GET: Admin/HomepageSettings
        public ActionResult HomepageSettings()
        {
            if (!(Session.Count > 0) || !((bool)Session["AdminLoggedIn"] == true))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // POST: Admin/HomepageSettings
        [HttpPost]
        public ActionResult HomepageSettings(FormCollection fc)
        {
            if (string.IsNullOrEmpty(fc["inputCapital"]) | string.IsNullOrEmpty(fc["inputDescribe"]) | string.IsNullOrEmpty(fc["buttonText"]) | string.IsNullOrEmpty(fc["buttonUrl"]))
            {
                //任何一项为空
                return View();
            }
            Jumbotron jb = new Jumbotron()
            {
                Capital = fc["inputCapital"],
                Describe = fc["inputDescribe"],
                DownloadButtonText = fc["buttonText"],
                DownloadUrl = fc["buttonUrl"]
            };
            db.Jumbotrons.Add(jb);
            db.SaveChanges();
            return View();
        }

        //
        // GET: Admin/AboutSettings
        public ActionResult AboutSettings()
        {
            //if (!(Session.Count > 0) || !((bool)Session["AdminLoggedIn"] == true))
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            return View(db.AboutItems.ToList());
        }

        //
        // POST: Admin/UpdateAboutImage
        [HttpPost]
        public ActionResult UpdateAboutImage(HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                const string fileTypes = "gif,jpg,jpeg,png,bmp";
                const int maxSize = 205000;
                var imgPath = "~/Images/itsme.jpg";
                if (image.ContentLength > maxSize)
                {
                    //超大
                    return RedirectToAction("AboutSettings");
                }
                var fileExt = Path.GetExtension(image.FileName);
                if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    //扩展名不匹配
                    return RedirectToAction("AboutSettings");
                }
                image.SaveAs(Server.MapPath(imgPath));
            }
            return RedirectToAction("AboutSettings");

        }

        //
        // GET: Admin/AddAboutItem
        public ActionResult AddAboutItem()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Admin/_PartialAboutItemAdder.cshtml");
            }
            else
            {
                return RedirectToAction("AboutSettings");
            }
        }

        //
        // POST:Admin/AddAboutItem
        [HttpPost]
        public ActionResult AddAboutItem(FormCollection fc)
        {
            string text = fc["inputText"];
            string value = fc["inputValue"];
            db.AboutItems.Add(new AboutItem { Text = text, Value = value });
            db.SaveChanges();
            return RedirectToAction("AboutSettings");
        }

        //
        // GET: Admin/DeleteAboutItem
        public ActionResult DeleteAboutItem(int ID)
        {
            AboutItem ai = db.AboutItems.Find(ID);
            if (ai != null)
            {
                db.AboutItems.Remove(ai);
                db.SaveChanges();
            }
            return RedirectToAction("AboutSettings");
        }

        //
        // GET: Admin/PreEdit
        public ActionResult PreEditAboutItem(int ID)
        {
            AboutItem ai = db.AboutItems.Find(ID);
            if (ai != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Admin/_PartialAboutItemEditor.cshtml", ai);
                }
            }
            return RedirectToAction("AboutSettings");
        }

        //
        // POST: Admin/EditAboutItem
        [HttpPost]
        public ActionResult EditAboutItem(FormCollection fc)
        {
            int ID = Convert.ToInt32(fc["hiddenID"]);
            string Text = fc["inputText"];
            string Value = fc["inputValue"];
            if (string.IsNullOrEmpty(Text) | string.IsNullOrEmpty(Value))
            {
                return View();
            }
            AboutItem ai = new AboutItem
            {
                ID = ID,
                Text = Text,
                Value = Value
            };
            db.Entry(ai).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AboutSettings");

        }

        //
        // Child Only: Show the sidebar
        [ChildActionOnly]
        public ActionResult ShowSidebar(int index)
        {
            var itemList = db.AdminSidebarItems.ToList();

            return PartialView("~/Views/Shared/_PartialAdminSidebar.cshtml", new SidebarItemVM(itemList, index));
        }
    }
}