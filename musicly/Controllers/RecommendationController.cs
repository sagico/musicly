﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace musicly.Controllers
{
    public class RecommendationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}