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

            AdminHomeVM ahvm = new AdminHomeVM
            {
                adminName = db.AdminAccounts.FirstOrDefault().Account,
                blogCount = db.BlogPages.Count(),
                programCount=db.Programs.Count(),
                editionCount=db.ProgramEditions.Count(),
                programList=db.Programs.ToList()
            };
            
            return View(ahvm);
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
            if (string.IsNullOrEmpty(Text) | string.IsNullOrEmpty(Action) | string.IsNullOrEmpty(Ctrl))
            {
                return View();
            }
            NavbarItem ni = new NavbarItem
            {
                ID = ID,
                Text = Text,
                Action = Action,
                Route = Ctrl
            };
            db.Entry(ni).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("NavbarSettings");

        }

        //
        // Navbar Index: 6
        // GET: Admin/ProgramSettings
        public ActionResult ProgramSettings()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            var programList = db.Programs.ToList();
            return View(programList);
        }

        //
        // Navbar Index: 6
        // GET: Admin/AddProgram
        public ActionResult AddProgram()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Admin/_PartialProgramAdder.cshtml");
            }
            else
            {
                return RedirectToAction("ProgramSettings");
            }
        }

        //
        // Navbar Index: 6
        // POST:Admin/AddProgram
        [HttpPost]
        public ActionResult AddProgram(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            string ename = fc["inputEname"];
            string cname = fc["inputCname"];
            string motive = fc["inputMotive"];
            string describe = fc["inputDescribe"];
            DateTime publish = DateTime.Now;
            string edStr = fc["inputEditionString"];
            string dUrl = fc["inputDownloadUrl"];
            db.Programs.Add(new Program
            {
                Ename = ename,
                Cname = cname,
                Motive = motive,
                Describe = describe,
                ProgramEditions = new List<ProgramEdition>
                {
                    new ProgramEdition
                    {
                        PublishDate=publish,
                        EditionString=edStr,
                        DownloadUrl=dUrl
                    }
                }
            });
            db.SaveChanges();
            return RedirectToAction("ProgramSettings");
        }

        //
        // Navbar Index: 6
        // GET: Admin/DeleteProgram
        public ActionResult DeleteProgram(int ID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            Program p = db.Programs.Find(ID);
            if (p != null)
            {
                db.Programs.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("ProgramSettings");
        }

        //
        // Navbar Index: 6
        // GET: Admin/PreEditProgram
        public ActionResult PreEditProgram(int ID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            Program p = db.Programs.Find(ID);
            if (p != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Admin/_PartialProgramEditor.cshtml", p);
                }
            }
            return RedirectToAction("ProgramSettings");
        }

        //
        // Navbar Index: 6
        // POST: Admin/EditProgram
        [HttpPost]
        public ActionResult EditProgram(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            int ID = Convert.ToInt32(fc["hiddenID"]);
            string Ename = fc["inputEname"];
            string Cname = fc["inputCname"];
            string Motive = fc["inputMotive"];
            string Describe = fc["inputDescribe"];
            if (string.IsNullOrEmpty(Ename) | string.IsNullOrEmpty(Cname) | string.IsNullOrEmpty(Motive) | string.IsNullOrEmpty(Describe))
            {
                return View();
            }
            Program p = new Program
            {
                ID = ID,
                Ename = Ename,
                Cname = Cname,
                Motive = Motive,
                Describe = Describe
            };
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ProgramSettings");

        }

        //
        // Navbar Index: 7
        // GET: Admin/BlogSettings
        public ActionResult BlogSettings()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            var blogList = db.BlogPages.ToList();
            return View(blogList);
        }

        //
        // Navbar Index: 7
        // GET: Admin/AddNewBlog
        public ActionResult AddNewBlog()
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // Navbar Index: 7
        // POST: Admin/AddNewBlog
        [HttpPost,ValidateInput(false)]
        public ActionResult AddNewBlog(FormCollection fc, HttpPostedFileBase image)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            //验证
            string title = fc["inputTitle"];
            string summary = fc["inputSketch"];
            //string imageInput = fc["image"];
            string content = fc["inputMD-markdown-doc"];
            string imgPath = "";
            if (string.IsNullOrEmpty(title) | string.IsNullOrEmpty(summary) | image == null | string.IsNullOrEmpty(content))
            {
                return View();
            }
            //保存图片
            if (image != null && image.ContentLength > 0)
            {
                const string fileTypes = "gif,jpg,jpeg,png,bmp";
                const int maxSize = 205000;
                string fileName = Guid.NewGuid().ToString();
                imgPath = $"/Images/{fileName}.jpg";
                if (image.ContentLength > maxSize)
                {
                    //超大
                    return RedirectToAction("AddNewBlog");
                }
                var fileExt = Path.GetExtension(image.FileName);
                if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    //扩展名不匹配
                    return RedirectToAction("AddNewBlog");
                }
                image.SaveAs(Server.MapPath(imgPath));
            }
            //保存文章
            BlogPage bp = new BlogPage
            {
                Title = title,
                Sketch = summary,
                CreateTime = DateTime.Now,
                ImagePath = imgPath,
                Content = content
            };
            db.BlogPages.Add(bp);
            db.SaveChanges();

            return RedirectToAction("BlogSettings");
        }

        //
        // Navbar Index: 7
        // GET: Admin/EditBlog
        public ActionResult EditBlog(int ID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            var blog = db.BlogPages.Find(ID);
            return View(blog);
        }

        //
        // Navbar Index: 7
        // POST: Admin/EditBlog
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBlog(FormCollection fc, HttpPostedFileBase image)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            //验证
            int ID = Convert.ToInt32(fc["hiddenID"]);
            string title = fc["inputTitle"];
            string summary = fc["inputSketch"];
            //string imageInput = fc["image"];
            string content = fc["inputMD-markdown-doc"];
            string imgPath = "";
            if (string.IsNullOrEmpty(title) | string.IsNullOrEmpty(summary) | string.IsNullOrEmpty(content))
            {
                return View();
            }
            //保存图片
            if (image != null && image.ContentLength > 0)
            {
                const string fileTypes = "gif,jpg,jpeg,png,bmp";
                const int maxSize = 205000;
                string fileName = Guid.NewGuid().ToString();
                imgPath = $"/Images/{fileName}.jpg";
                if (image.ContentLength > maxSize)
                {
                    //超大
                    return RedirectToAction("EditBlog", new { ID = ID });
                }
                var fileExt = Path.GetExtension(image.FileName);
                if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    //扩展名不匹配
                    return RedirectToAction("EditBlog", new { ID = ID });
                }
                image.SaveAs(Server.MapPath(imgPath));
            }
            //保存文章
            var bp = db.BlogPages.Find(ID);
            if (bp != null)
            {
                //ID号存在
                bp.Title = title;
                bp.Sketch = summary;
                bp.CreateTime = DateTime.Now;
                bp.Content = content;
                if (imgPath != "")
                {
                    bp.ImagePath = imgPath;
                }
            }
            db.Entry(bp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("BlogSettings");
        }

        //
        // Navbar Index: 7
        // GET: Admin/DeleteBlog
        public ActionResult DeleteBlog(int ID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            var bp = db.BlogPages.Find(ID);
            if (bp != null)
            {
                db.BlogPages.Remove(bp);
                db.SaveChanges();
            }
            return RedirectToAction("BlogSettings");
        }

        //
        // Navbar Index: 7
        // POST: Admin/QueryBlogs
        [HttpPost]
        public ActionResult QueryBlogs(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            string key = fc["inputKey"];
            if (string.IsNullOrEmpty(key))
            {
                return RedirectToAction("BlogSettings");
            }

            var blogs = db.BlogPages.Where(
                b => b.Title.IndexOf(key) >= 0 || b.Sketch.IndexOf(key) >= 0 || b.Content.IndexOf(key) >= 0
            ).ToList();
            return View(blogs);
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
        // Navbar Index: null
        // GET: Admin/EditonSettings
        public ActionResult EditionSettings(int programID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            var editionList = db.ProgramEditions.Where(pe => pe.ProgramID == programID).ToList();
            return View(editionList);
        }

        //
        // Navbar Index: null
        // GET: Admin/AddProgramEdition
        public ActionResult AddProgramEdition(int programID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Admin/_PartialProgramEditionAdder.cshtml", db.Programs.Find(programID));
            }
            else
            {
                return RedirectToAction("EditionSettings", new { programID = programID });
            }
        }

        //
        // Navbar Index: null
        // POST:Admin/AddProgramEdition
        [HttpPost]
        public ActionResult AddProgramEdition(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            int programID = Convert.ToInt32(fc["hiddenID"]);
            DateTime publish = Convert.ToDateTime(fc["inputDate"]);
            string edStr = fc["inputEditionString"];
            string dUrl = fc["inputDownloadUrl"];
            db.Programs.Find(programID).ProgramEditions.Add(
                    new ProgramEdition
                    {
                        PublishDate = publish,
                        EditionString = edStr,
                        DownloadUrl = dUrl
                    }
            );
            db.SaveChanges();
            return RedirectToAction("EditionSettings", new { programID = programID });
        }

        //
        // Navbar Index: null
        // GET: Admin/DeleteProgramEdition
        public ActionResult DeleteProgramEdition(int ID, int programID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            ProgramEdition pe = db.ProgramEditions.Find(ID);
            if (pe != null && db.ProgramEditions.Where(p => p.ProgramID == programID).Count() > 1)
            {
                db.ProgramEditions.Remove(pe);
                db.SaveChanges();
            }
            return RedirectToAction("EditionSettings", new { programID = programID });
        }

        //
        // Navbar Index: null
        // GET: Admin/PreEditProgramEdition
        public ActionResult PreEditProgramEdition(int ID, int programID)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            ProgramEdition pe = db.Programs.Find(programID).ProgramEditions.Find(pp => pp.ID == ID);
            if (pe != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Admin/_PartialProgramEditionEditor.cshtml", pe);
                }
            }
            return RedirectToAction("EditionSettings", new { programID = programID });
        }

        //
        // Navbar Index: null
        // POST: Admin/EditProgramEdition
        [HttpPost]
        public ActionResult EditProgramEdition(FormCollection fc)
        {
            if (!CheckLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            int programID = Convert.ToInt32(fc["hiddenProgramID"]);
            int ID = Convert.ToInt32(fc["hiddenID"]);
            DateTime publish = Convert.ToDateTime(fc["inputDate"]);
            string edStr = fc["inputEditionString"];
            string dUrl = fc["inputDownloadUrl"];
            if (string.IsNullOrEmpty(edStr) | string.IsNullOrEmpty(dUrl))
            {
                return View();
            }
            ProgramEdition pe = new ProgramEdition
            {
                ID = ID,
                ProgramID = programID,
                PublishDate = publish,
                EditionString = edStr,
                DownloadUrl = dUrl
            };

            db.Entry(pe).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("EditionSettings", new { programID = programID });

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