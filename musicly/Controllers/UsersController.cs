﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using musicly.Data;
using musicly.Models;

namespace musicly.Controllers
{
    public class UsersController : Controller
    {
        private readonly musiclyContext _context;

        public UsersController(musiclyContext context)
        {
            
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string searchString)
        {
            Authorize();
            var users = from i in _context.User select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString));
            }
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            AuthorizeUser();

            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetInt32("UserId") != id && HttpContext.Session.GetInt32("Admin") == null)
                throw new UnauthorizedAccessException();

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            Authorize();
          
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,FirstName,LastName,BirthDate,City,IsAdmin")] User user)
        {
            Authorize();
            if (ModelState.IsValid && _context.User.Where(usert=> usert.UserName == user.UserName) == null)
            {                
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AuthorizeUser();
           
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,FirstName,LastName,BirthDate,City,IsAdmin")] User user)
        {
            AuthorizeUser();

            if (HttpContext.Session.GetInt32("UserId") != id && HttpContext.Session.GetInt32("Admin") == null)
                throw new UnauthorizedAccessException();

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (HttpContext.Session.GetInt32("UserId") == id)
                    return Redirect("/home");
                else
                    return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Authorize();
            if (HttpContext.Session.GetInt32("UserId") == id)
                throw new UnauthorizedAccessException();

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Authorize();
            if (HttpContext.Session.GetInt32("UserId") == id)
                throw new UnauthorizedAccessException();

            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        private void Authorize()
        {
            if (HttpContext.Session.GetInt32("Admin") == null)
                throw new UnauthorizedAccessException();
        }

        private void AuthorizeUser()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                throw new UnauthorizedAccessException();
        }
    }
}
