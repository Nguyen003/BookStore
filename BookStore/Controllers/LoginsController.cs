using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class LoginsController : Controller
    {
        private DbBookStore db = new DbBookStore();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var ten = collection["username"];
            var pass = collection["password"];
            if(String.IsNullOrEmpty(ten))
            {
                ViewData["Loi1"] = "Nhập tên người dùng";
            } else if(String.IsNullOrEmpty(pass))
            {
                ViewData["Loi2"] = "Nhập mật khẩu";
            }
            else
            {
                User user = db.Users.SingleOrDefault(n => n.Username == ten && n.Password == pass);
                if(user != null)
                {
                    Session["userLogin"] = user;
                    return RedirectToAction("Index", "Book");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}