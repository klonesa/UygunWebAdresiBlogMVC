using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Data.Entities;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        //Role inactive
        private UygunWebAdresiBlogEntities _db = new UygunWebAdresiBlogEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users user)
        {
            var result = _db.Users.SingleOrDefault(x => x.UserEmail == user.UserEmail);
            if (result != null && result.UserEmail == user.UserEmail && result.UserPassword == user.UserPassword)
            {
                Session["id"] = result.UserId;
                Session["email"] = result.UserEmail;
                return RedirectToAction("Index", "Admin");
            }

            @ViewBag.FailedLogin = "Hatalı Giriş ! Bilgilerinizi kontrol ediniz..."; ;
            return View(user);
        }

        public ActionResult Logout()
        {
            Session["id"] = null;
            Session["email"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}