using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicly.Data;
using musicly.Models;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace musicly.Controllers
{
    public class InstrumentOrdersController : Controller
    {
        private readonly musiclyContext _context;
        private const int NUMBER_OF_RECOMMENDATION = 6;
        public InstrumentOrdersController(musiclyContext context)
        {
            _context = context;
        }

        // GET: InstrumentOrders
        public async Task<IActionResult> Index(int? searchId)
        {
            var musiclyContext = _context.InstrumentOrder.Include(i => i.Instrument).Include(i => i.Order);
            var orders = from i in musiclyContext select i;

            if (searchId != null)
            {
                orders = orders.Where(i => i.InstrumentOrderID == searchId);
            }

            return View(await orders.ToListAsync());
        }

        // GET: Recommendation
        [Route("Recommendations")]
        public IActionResult GetRecommendationByInstrument(int? instrumentId)
        {
            int count = 0;
            var recommendations = new List<Instrument>();
            var instrumentOrders = _context.InstrumentOrder.Include(i=>i.Order).Include(i=>i.Instrument).ToList();
            var orderIdList = new List<int>();
            
            if (instrumentId == null)
            {
                var baseUser = _context.User.FirstOrDefault(m => m.Id == HttpContext.Session.GetInt32("UserId"));
                DateTime fiveYearsBefore = baseUser.BirthDate.AddYears(-5), fiveYearsLater = baseUser.BirthDate.AddYears(5);

                var usersInAge = _context.User.ToList().Where(tempUser =>
                                            (tempUser.BirthDate >= fiveYearsBefore && tempUser.BirthDate <= fiveYearsLater)).Select(user => user.Id).ToList();
                orderIdList = instrumentOrders.Where(index => usersInAge.Contains(index.Order.UserId)).Select(order => order.OrderId).ToList();
            } 
            else
            {
                orderIdList = instrumentOrders.Where(i => i.InstrumentId == instrumentId).Select(i => i.OrderId).ToList();
            }

            orderIdList = orderIdList.Distinct().ToList();

            foreach (var instrumentOrder in instrumentOrders.OrderBy(order=> order.Quantity))
            {
                if (orderIdList.Contains(instrumentOrder.OrderId) && !recommendations.Contains(instrumentOrder.Instrument))
                {
                    recommendations.Add(instrumentOrder.Instrument);
                    if (++count == NUMBER_OF_RECOMMENDATION)
                        break;
                }               
            }

            return Ok(recommendations.Select(i=>new { i.Id, i.ImagePath, i.Name, i.Price}));
        }

        // POST: Instruments/order
        [Route("Instruments/order")]
        [HttpPost]
        public IActionResult createOrder(IEnumerable<CartItem> items)
        {
            try
            {
                if (items.Count() == 0)
                    throw new Exception();

                Order newOrder = new Order()
                {
                    Id = 0,
                    OrderDate = DateTime.Now,
                    UserId = (int)HttpContext.Session.GetInt32("UserId")
                };

                _context.Add(newOrder);
                _context.SaveChanges();
                int orderId = _context.Order.Max(order => order.Id);
                foreach (var cartItem in items)
                {
                    var instrumentOrder = new InstrumentOrder()
                    {
                        InstrumentId = cartItem.Id,
                        Quantity = cartItem.Quantity,
                        OrderId = (int)orderId
                    };

                    _context.Add(instrumentOrder);
                }
                _context.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
        
            return Ok();              
        }

        // GET: InstrumentOrdersController/Cart
        [Route("InstrumentOrdersController/Cart")]
        public async Task<IActionResult> Cart(int? searchId)
        {
            return View();
        }

        // GET: InstrumentOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentOrder = await _context.InstrumentOrder
                .Include(i => i.Instrument)
                .Include(i => i.Order)
                .FirstOrDefaultAsync(m => m.InstrumentOrderID == id);
            if (instrumentOrder == null)
            {
                return NotFound();
            }

            return View(instrumentOrder);
        }

        // GET: InstrumentOrders/Create
        public IActionResult Create()
        {
            ViewData["InstrumentId"] = new SelectList(_context.Set<Instrument>(), "Id", "Id");
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id");
            return View();
        }

        // POST: InstrumentOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentOrderID,OrderId,InstrumentId,Quantity")] InstrumentOrder instrumentOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instrumentOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentId"] = new SelectList(_context.Set<Instrument>(), "Id", "Id", instrumentOrder.InstrumentId);
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", instrumentOrder.OrderId);
            return View(instrumentOrder);
        }

        // GET: InstrumentOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentOrder = await _context.InstrumentOrder.FindAsync(id);
            if (instrumentOrder == null)
            {
                return NotFound();
            }
            ViewData["InstrumentId"] = new SelectList(_context.Set<Instrument>(), "Id", "Id", instrumentOrder.InstrumentId);
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", instrumentOrder.OrderId);
            return View(instrumentOrder);
        }

        // POST: InstrumentOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentOrderID,OrderId,InstrumentId,Quantity")] InstrumentOrder instrumentOrder)
        {
            if (id != instrumentOrder.InstrumentOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrumentOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentOrderExists(instrumentOrder.InstrumentOrderID))
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
            ViewData["InstrumentId"] = new SelectList(_context.Set<Instrument>(), "Id", "Id", instrumentOrder.InstrumentId);
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", instrumentOrder.OrderId);
            return View(instrumentOrder);
        }

        // GET: InstrumentOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentOrder = await _context.InstrumentOrder
                .Include(i => i.Instrument)
                .Include(i => i.Order)
                .FirstOrDefaultAsync(m => m.InstrumentOrderID == id);
            if (instrumentOrder == null)
            {
                return NotFound();
            }

            return View(instrumentOrder);
        }

        // POST: InstrumentOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrumentOrder = await _context.InstrumentOrder.FindAsync(id);
            _context.InstrumentOrder.Remove(instrumentOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstrumentOrderExists(int id)
        {
            return _context.InstrumentOrder.Any(e => e.InstrumentOrderID == id);
        }
    }
}
