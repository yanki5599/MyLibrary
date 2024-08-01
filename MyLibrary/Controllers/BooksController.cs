using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Exceptions;
using MyLibrary.Models;
using MyLibrary.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace MyLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly MyLibraryContext _context;

        public BooksController(MyLibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string? messageStr = null)
        {
            var myLibraryContext = _context.Book.Include(b => b.SetOfBooks).Include(b => b.Shelf).ThenInclude(b => b.Category);
            ViewData["messege"] = messageStr;
            return View(await myLibraryContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.SetOfBooks)
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create(int? id)
        {
            if (id != null)
            {
                var currShelf = _context.Shelf.Include(s => s.Category).First(s => s.Id == id);
                var list = new List<Shelf>();
                list.Add(currShelf);
                ViewData["CategoryName"] =new SelectList(list,"Id", "Category.CategoryName",currShelf.Id);
            }
            else
            {
                ViewData["CategoryName"] =
                new SelectList(_context.Category, "Id", "CategoryName");

            }

            return View(new BooksVM());
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BooksVM booksVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Shelf shelf = await GetMatchingShelfAsync(booksVm);
                   
                    if (booksVm.IsSet)
                    {
                        AddSetToDB(booksVm , shelf); 
                    }
                    else
                    {
                        booksVm.FirstBook.Height = booksVm.Height;
                        var book = new Book(booksVm.FirstBook, shelf.Id);
                        _context.Add(book);
                    }

                    shelf.AvailableSpace -= booksVm.TotalWidth;
                    _context.Update(shelf);
                    await _context.SaveChangesAsync();
                    

                    string? message = null;
                    if (shelf.Height - booksVm.Height >= 10)
                    {
                        message =  "שים לב!! יש 10 סמ או יותר מרווח בגובה של המדף";
                    }
                    return RedirectToAction(nameof(Index), new { messageStr = message });
                }
                catch(BookCreateFail ex)
                {
                    ViewData["errors"] = ex.Message;
                }
                catch (Exception ex) 
                {
                    throw;
                }
                
            }
            ViewData["CategoryName"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View(booksVm);
        }

        private void AddSetToDB(BooksVM booksVm , Shelf shelf)
        {
            var newSetOfBooks = new SetOfBooks() { Name = booksVm.SetOfBooksName };
            _context.Add(newSetOfBooks);
            _context.SaveChanges();
            var setId = newSetOfBooks.Id;

            foreach (var bookVm in booksVm.Books)
            {
                bookVm.Height = booksVm.Height;
                var book = new Book(bookVm, shelf.Id, setId);
                _context.Add(book);
            }
        }

        private async Task<Shelf> GetMatchingShelfAsync(BooksVM booksVm)
        {
            var shelf = await _context.Shelf.FirstOrDefaultAsync(sh => sh.CategoryId == booksVm.CategoryId
                                                                  && sh.AvailableSpace >= booksVm.TotalWidth &&
                                                                  sh.Height >= booksVm.Height);
           
            if (shelf == null)
                throw new BookCreateFail("There is no shelf available , try adding new shelfs.");

            return shelf;
        }


        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.SetOfBooks)
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                var shelf = await _context.Shelf.FirstOrDefaultAsync(b => b.Id == book.ShelfId);
                shelf.AvailableSpace += book.Width;
                _context.Book.Remove(book);
                _context.Update(shelf);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
