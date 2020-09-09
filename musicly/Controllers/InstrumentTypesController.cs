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
    public class InstrumentTypesController : Controller
    {
        private readonly musiclyContext _context;

        public InstrumentTypesController(musiclyContext context)
        {
            _context = context;
        }

        // GET: InstrumentTypes
        public async Task<IActionResult> Index(string searchString)
        {
            UserAuthorization();

            var instrumentTypes = from i in _context.InstrumentType select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                instrumentTypes = instrumentTypes.Where(i => i.Name.Contains(searchString));
            }

            return View(await instrumentTypes.ToListAsync());
        }

        [Route("Instruments/Types")]
        public async Task<IActionResult> GetAllInstrumentTypes()
        {
            var instrumentTypes =  _context.InstrumentType.ToArray();
            if (instrumentTypes == null)
            {
                return NotFound();
            }
            return Ok(instrumentTypes);
        }

        // GET: InstrumentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            UserAuthorization();

            if (id == null)
            {
                return NotFound();
            }

            var instrumentType = await _context.InstrumentType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instrumentType == null)
            {
                return NotFound();
            }

            return View(instrumentType);
        }

        // GET: InstrumentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InstrumentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] InstrumentType instrumentType)
        {
            AdminAuthorization();

            if (ModelState.IsValid)
            {
                _context.Add(instrumentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instrumentType);
        }

        // GET: InstrumentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AdminAuthorization();

            if (id == null)
            {
                return NotFound();
            }

            var instrumentType = await _context.InstrumentType.FindAsync(id);
            if (instrumentType == null)
            {
                return NotFound();
            }
            return View(instrumentType);
        }

        // POST: InstrumentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] InstrumentType instrumentType)
        {
            AdminAuthorization();

            if (id != instrumentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrumentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentTypeExists(instrumentType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instrumentType);
        }

        // GET: InstrumentTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            AdminAuthorization();

            if (id == null)
            {
                return NotFound();
            }

            var instrumentType = await _context.InstrumentType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instrumentType == null)
            {
                return NotFound();
            }

            return View(instrumentType);
        }

        // POST: InstrumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            AdminAuthorization();

            var instrumentType = await _context.InstrumentType.FindAsync(id);
            _context.InstrumentType.Remove(instrumentType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstrumentTypeExists(int id)
        {
            UserAuthorization();

            return _context.InstrumentType.Any(e => e.Id == id);
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
