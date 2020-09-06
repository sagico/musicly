using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicly.Data;
using musicly.Models;

namespace musicly.Controllers
{
    public class InstrumentsController : Controller
    {
        private readonly musiclyContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public InstrumentsController(musiclyContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hostingEnvironment = hosting;
        }

        private void SaveImage(Instrument instrument, IFormFile file)
        {
            AdminAuthorization();

            var fileName = Path.Combine(_hostingEnvironment.ContentRootPath + "\\images\\instruments", Path.GetFileName(file.FileName));
            instrument.ImagePath = "/images/apartments/" + file.FileName;

            // If the file does not exist already creating it
            file.CopyTo(new FileStream(fileName, FileMode.Create));
        }

        // GET: instrumentImage
        [Route("Images/{imageName}")]
        public async Task<IActionResult> getImage(string imageName)
        {
            UserAuthorization();

            var image = System.IO.File.OpenRead("./Views/Instruments/Images/" + imageName);
            return File(image, "image/jpg");

        }

        // GET: Instruments
        public async Task<IActionResult> Index(string searchString)
        {
            UserAuthorization();
            var musiclyContext = _context.Instrument.Include(i => i.InstrumentType);
            var instruments = from i in musiclyContext select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                instruments = instruments.Where(i => i.Name.Contains(searchString));
            }

            return View(await instruments.ToListAsync());
        }

        // GET: Instruments/list
        [Route("Instruments/list")]
        public IActionResult getInstruments()
        {
            UserAuthorization();
            var musiclyContext = _context.Instrument.Include(i => i.InstrumentType);
            return Ok(musiclyContext);
        }

        // GET: Instruments/list
        public async Task<IActionResult> InstrumentsList()
        {
            UserAuthorization();
            return View();
        }


        // GET: Instruments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            UserAuthorization();

            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument
                .Include(i => i.InstrumentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
        }

        // GET: Instruments/Create
        public IActionResult Create()
        {
            AdminAuthorization();

            ViewData["TypeID"] = new SelectList(_context.InstrumentType, "Id", "Name");
            return View();
        }

        // POST: Instruments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,ImagePath,TypeID")] Instrument instrument, IFormFile file)
        {
            AdminAuthorization();

            if (ModelState.IsValid)
            {
                SaveImage(instrument, file);
                _context.Add(instrument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeID"] = new SelectList(_context.InstrumentType, "Id", "Name", instrument.TypeID);
            return View(instrument);
        }

        // GET: Instruments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AdminAuthorization();

            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument.FindAsync(id);
            if (instrument == null)
            {
                return NotFound();
            }
            ViewData["TypeID"] = new SelectList(_context.InstrumentType, "Id", "Id", instrument.TypeID);
            return View(instrument);
        }

        // POST: Instruments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ImagePath,TypeID")] Instrument instrument)
        {
            AdminAuthorization();
            if (id != instrument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentExists(instrument.Id))
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
            ViewData["TypeID"] = new SelectList(_context.InstrumentType, "Id", "Id", instrument.TypeID);
            return View(instrument);
        }

        // GET: Instruments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            AdminAuthorization();
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument
                .Include(i => i.InstrumentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
        }

        // POST: Instruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            AdminAuthorization();
            var instrument = await _context.Instrument.FindAsync(id);
            _context.Instrument.Remove(instrument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstrumentExists(int id)
        {
            UserAuthorization();
            return _context.Instrument.Any(e => e.Id == id);
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
