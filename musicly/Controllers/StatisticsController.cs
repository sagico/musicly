using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace musicly.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            Authorize();
            return View();
        }

        private void Authorize()
        {
            if (HttpContext.Session.GetInt32("Admin") == null)
                throw new UnauthorizedAccessException();
        }
    }
}