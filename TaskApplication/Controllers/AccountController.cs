using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskApplication.Models;

namespace TaskApplication.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User UserTable)
        {
            if(ModelState.IsValid)
            {
                using (var t = new TaskContext())
                {
                    User user = t.Users.Where(u => u.UserName == UserTable.UserName & UserTable.Password == UserTable.Password).FirstOrDefault();
                   
                    if(user != null)
                    {
                        Session["UserName"] = user.UserName;
                        Session["UserId"] = user.Id;
                        return RedirectToAction("Index", "Home");
                        //return RedirectToRoute("");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid UserName And Password ");
                        return View(UserTable);
                    }
                }
            }
            else
            {
                return View(UserTable);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Register(User UserTable)
        {
            if(UsernameExists(UserTable.UserName))
            {
                ModelState.AddModelError("", "Username already exists!");
                return View(UserTable);
            }
            using (var db = new TaskContext())
            {
                var user = new User();

                user.UserName = UserTable.UserName;
                user.Password = UserTable.Password;
                user.RoleId = 1;

                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("Login", "Account");
            }
        }
        private bool UsernameExists(string userName)
        {
            using (var db = new TaskContext())
            {
                var result = db.Users.Where(u => u.UserName == userName).FirstOrDefault();

                if (result != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
    
}
