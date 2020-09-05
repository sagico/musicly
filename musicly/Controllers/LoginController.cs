using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using musicly.Data;
using musicly.Models;

namespace musicly.Controllers
{
    public class LoginController : Controller
    {
        private readonly musiclyContext _context;

        public LoginController(musiclyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string username, string password)
        {
            User user = _context.User.SingleOrDefault(User => User.UserName == username && User.Password == password);
            if (user == null)
            {
                ViewBag.Message = "Invalid username or password";
                return View();
            }
            if (user.IsAdmin)
            {
                HttpContext.Session.SetInt32("Admin",1);
            }

            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("FirstName", user.FirstName);

            return Redirect("/Home/");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Home/");
        }
    }
}
