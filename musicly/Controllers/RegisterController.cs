using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using musicly.Data;
using musicly.Models;

namespace musicly.Controllers
{
    public class RegisterController : Controller
    {
        private readonly musiclyContext _context;

        public RegisterController(musiclyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("FirstName,LastName,BirthDate,UserName,Password,City")] User user)
        {
            if (ModelState.IsValid)
            {
                User existUser = _context.User.SingleOrDefault(User => User.UserName == user.UserName);
                if (existUser == null)
                {
                    user.IsAdmin = false;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ErrorMsg = "User Already Exist!";
                return View();

            }
            return View(user);
        }
    }
}
