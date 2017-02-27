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
        // Navbar Index: 1
        // GET: Admin
        public ActionResult Index()
        {
            if (!CheckLogin())
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
        // Navbar Index: 2
        // GET: Admin/WebsiteSettings
        public ActionResult WebsiteSettings()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            MainSite ms = db.MainSites.FirstOrDefault();

            return View(ms);
        }

        //
        // Navbar Index: 2
        // POST: Admin/WebsiteSettings
        [HttpPost]
        public ActionResult WebsiteSettings(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            string title = fc["inputTitle"];
            string main = fc["inputMain"];
            string sub = fc["inputSub"];
            if (string.IsNullOrEmpty(title) | string.IsNullOrEmpty(main) | string.IsNullOrEmpty(sub))
            {
                //任意一项为空
                return RedirectToAction("Index");
            }

            MainSite ms = db.MainSites.FirstOrDefault();
            ms.TitleText = title;
            ms.MainCapital = main;
            ms.SubCapital = sub;
            db.Entry(ms).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("WebsiteSettings");
        }

        //
        // Navbar Index: 3
        // GET: Admin/HomepageSettings
        public ActionResult HomepageSettings()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // Navbar Index: 3
        // POST: Admin/HomepageSettings
        [HttpPost]
        public ActionResult HomepageSettings(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
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
        // Navbar Index: 4
        // GET: Admin/AboutSettings
        public ActionResult AboutSettings()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.AboutItems.ToList());
        }

        //
        // Navbar Index: 4
        // POST: Admin/UpdateAboutImage
        [HttpPost]
        public ActionResult UpdateAboutImage(HttpPostedFileBase image)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
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
        // Navbar Index: 4
        // GET: Admin/AddAboutItem
        public ActionResult AddAboutItem()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
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
        // Navbar Index: 4
        // POST:Admin/AddAboutItem
        [HttpPost]
        public ActionResult AddAboutItem(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            string text = fc["inputText"];
            string value = fc["inputValue"];
            db.AboutItems.Add(new AboutItem { Text = text, Value = value });
            db.SaveChanges();
            return RedirectToAction("AboutSettings");
        }

        //
        // Navbar Index: 4
        // GET: Admin/DeleteAboutItem
        public ActionResult DeleteAboutItem(int ID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            AboutItem ai = db.AboutItems.Find(ID);
            if (ai != null)
            {
                db.AboutItems.Remove(ai);
                db.SaveChanges();
            }
            return RedirectToAction("AboutSettings");
        }

        //
        // Navbar Index: 4
        // GET: Admin/PreEditAboutItem
        public ActionResult PreEditAboutItem(int ID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
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
        // Navbar Index: 4
        // POST: Admin/EditAboutItem
        [HttpPost]
        public ActionResult EditAboutItem(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
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
        // Navbar Index: 5
        // GET: Admin/NavbarSettings
        public ActionResult NavbarSettings()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            List<NavbarItem> list = db.NavbarItems.ToList();
            return View(list);
        }

        //
        // Navbar Index: 5
        // GET: Admin/AddNavbarItem
        public ActionResult AddNavbarItem()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Admin/_PartialNavbarItemAdder.cshtml");
            }
            else
            {
                return RedirectToAction("NavbarSettings");
            }
        }

        //
        // Navbar Index: 5
        // POST:Admin/AddNavbarItem
        [HttpPost]
        public ActionResult AddNavbarItem(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            string text = fc["inputText"];
            string action = fc["inputAction"];
            string ctrl = fc["inputController"];
            db.NavbarItems.Add(new NavbarItem { Text = text, Action = action, Route = ctrl });
            db.SaveChanges();
            return RedirectToAction("NavbarSettings");
        }

        //
        // Navbar Index: 5
        // GET: Admin/DeleteNavbarItem
        public ActionResult DeleteNavbarItem(int ID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            NavbarItem ni = db.NavbarItems.Find(ID);
            if (ni != null)
            {
                db.NavbarItems.Remove(ni);
                db.SaveChanges();
            }
            return RedirectToAction("NavbarSettings");
        }

        //
        // Navbar Index: 5
        // GET: Admin/PreEditNavbarItem
        public ActionResult PreEditNavbarItem(int ID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            NavbarItem ni = db.NavbarItems.Find(ID);
            if (ni != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Admin/_PartialNavbarItemEditor.cshtml", ni);
                }
            }
            return RedirectToAction("NavbarSettings");
        }

        //
        // Navbar Index: 5
        // POST: Admin/EditNavbarItem
        [HttpPost]
        public ActionResult EditNavbarItem(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            int ID = Convert.ToInt32(fc["hiddenID"]);
            string Text = fc["inputText"];
            string Action = fc["inputAction"];
            string Ctrl = fc["inputController"];
            if (string.IsNullOrEmpty(Text) | string.IsNullOrEmpty(Action)|string.IsNullOrEmpty(Ctrl))
            {
                return View();
            }
            NavbarItem ni = new NavbarItem
            {
                ID = ID,
                Text = Text,
                Action=Action,
                Route=Ctrl
            };
            db.Entry(ni).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("NavbarSettings");

        }

        //
        // Navbar Index: 8
        // GET: Admin/AdminSettings
        public ActionResult AdminSettings()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // Navbar Index: 8
        // POST: Admin/AdminSettings
        [HttpPost]
        public ActionResult AdminSettings(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

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
        // Child Only: Show the sidebar
        [ChildActionOnly]
        public ActionResult ShowSidebar(int index)
        {
            var itemList = db.AdminSidebarItems.ToList();

            return PartialView("~/Views/Shared/_PartialAdminSidebar.cshtml", new SidebarItemVM(itemList, index));
        }

        //
        // Private Method
        private bool CheckLogin()
        {
            if (!(Session.Count > 0) || !((bool)Session["AdminLoggedIn"] == true))
            {
                return false;
            }
            return true;
        }
    }
}