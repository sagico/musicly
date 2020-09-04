using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicly.Data;
using musicly.Models;

namespace musicly.Controllers
{
    public class StoresController : Controller
    {
        private readonly musiclyContext _context;

        public StoresController(musiclyContext context)
        {
            _context = context;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Store.ToListAsync());
        }

       
    }
}
