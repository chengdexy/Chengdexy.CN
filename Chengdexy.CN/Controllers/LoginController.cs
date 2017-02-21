using Chengdexy.CN.DAL;
using Chengdexy.CN.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Chengdexy.CN.Controllers
{
    public class LoginController : Controller
    {
        private MainContext db = new MainContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            string accountInput = fc["inputAccount"];
            string passwordInput = MD5maker.GetMd5Hash(MD5.Create(), fc["inputPassword"]);

            var admin = db.AdminAccounts.Where(aa => aa.Account == accountInput && aa.Password == passwordInput);
            if (admin.Count() > 0)
            {
                //登录成功
                Session.Add("AdminLoggedIn", true);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                //登录失败
                return View();
            }

        }

        // GET: Login/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}