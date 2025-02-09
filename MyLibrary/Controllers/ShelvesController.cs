﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class ShelvesController : Controller
    {
        private readonly MyLibraryContext _context;

        public ShelvesController(MyLibraryContext context)
        {
            _context = context;
        }

        // GET: Shelves
        public async Task<IActionResult> Index()
        {
            var myLibraryContext = _context.Shelf.Include(s => s.Category);
            return View(await myLibraryContext.ToListAsync());
        }

        // GET: Shelves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelf = await _context.Shelf
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelf == null)
            {
                return NotFound();
            }

            return View(shelf);
        }

        // GET: Shelves/Create
        public IActionResult Create(int? id)
        {
            if (id != null)
            {
                var currCat = _context.Category.First(s => s.Id == id);
                var list = new List<Category>();
                list.Add(currCat);
                ViewData["CategoryName"] = new SelectList(list, "Id", "CategoryName", currCat.Id);
            }
            else
            {
                ViewData["CategoryName"] =
                new SelectList(_context.Category, "Id", "CategoryName");

            }

            //ViewData["CategoryName"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View();
        }

        // POST: Shelves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Width,Height,CategoryId")] Shelf shelf)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shelf);
                shelf.AvailableSpace = shelf.Width;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryName"] = new SelectList(_context.Category, "Id", "CategoryName", shelf.CategoryId);
            return View(shelf);
        }

       
        // GET: Shelves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelf = await _context.Shelf
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelf == null)
            {
                return NotFound();
            }

            return View(shelf);
        }

        // POST: Shelves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelf = await _context.Shelf.FindAsync(id);
            if (shelf != null)
            {
                _context.Shelf.Remove(shelf);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelfExists(int id)
        {
            return _context.Shelf.Any(e => e.Id == id);
        }
    }
}
