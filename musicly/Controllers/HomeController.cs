using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using musicly.Models;

namespace musicly.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var errorCode = "500";
            var description = "Internal Server Error";
            
            
            if(exceptionHandlerPathFeature == null)
            {
                errorCode = "404";
                description = "דף האינטרנט שאתה מחפש לא קיים";
            } 
            else if (exceptionHandlerPathFeature?.Error is UnauthorizedAccessException)
            {
                errorCode = "401";
                description = "אינך מורשה לבצע פעולה זאת";
            }

            return View(new ErrorViewModel { ErrorCode = errorCode, Description = description });
        }
    }
}
