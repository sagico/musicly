using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            return Redirect("/Login/Login");
        }

        public IActionResult Login(string username, string password)
        {
            if (username == null && password == null)
            {
                return View();
            }

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
