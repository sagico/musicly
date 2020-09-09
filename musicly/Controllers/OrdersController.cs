using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicly.Data;
using musicly.Models;

namespace musicly.Controllers
{
    public class OrdersController : Controller
    {
        private readonly musiclyContext _context;

        public OrdersController(musiclyContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            AdminAuthorization();
            var musiclyContext = _context.Order.Include(o => o.User)
                                               .Include(o => o.InstrumentOrders)
                                                    .ThenInclude(i => i.Instrument);
            return View(await musiclyContext.ToListAsync());
        }

        public async Task<IActionResult> OrdersForUser()
        {
            UserAuthorization();
            var musiclyContext = _context.Order.Include(o => o.User)
                                               .Include(o => o.InstrumentOrders)
                                                    .ThenInclude(i => i.Instrument)
                                               .Where(o => o.User.Id == (int)HttpContext.Session.GetInt32("UserId"));
            return View(await musiclyContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            AdminAuthorization();
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.User)
                    .Include(o => o.InstrumentOrders)
                            .ThenInclude(i => i.Instrument)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

       
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            AdminAuthorization();
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
        private void AdminAuthorization()
        {
            if (HttpContext.Session.GetInt32("Admin") == null)
                throw new UnauthorizedAccessException();
        }

        private void UserAuthorization()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                throw new UnauthorizedAccessException();
        }

    }
}
