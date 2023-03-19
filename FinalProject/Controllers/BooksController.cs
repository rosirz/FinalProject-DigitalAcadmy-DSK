using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.DataAccess;
using FinalProject.DataModels;

namespace FinalProject.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = _context.Books.Include(b => b.Author)
                .Include(b => b.Genre)
                .AsNoTracking();

            CreateAuthorDropDownList();
            CreateGenreDropDownList();
            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(a => a.Author)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var book = new Book();
            book.AuthorList = (ICollection<SelectListItem>)CreateAuthorDropDownList();
            book.GenreList = (ICollection<SelectListItem>)CreateGenreDropDownList();
            return View(book);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("BookId,Name,Description,AuthorId,GenreId")] Book book,
            [Bind("AuthorId")] Author author, [Bind("GenreId")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                CreateAuthorDropDownList();
                CreateGenreDropDownList();

                _context.Add(book);

                Author author1 = await _context.Authors.FirstAsync(a => a.AuthorId.Equals(book.AuthorId));
                author1.Books.Add(book);
                Genre genre1 = await _context.Genres.FirstAsync(g => g.GenreId.Equals(book.GenreId));
                genre1.Books.Add(book);

                AuthorGenres current = new AuthorGenres();
                current.Author = author1;
                current.Genre = genre1;
               
               if ( 
                   await _context.AuthorGenres.FindAsync(current.Author.AuthorId, current.Genre.GenreId) != null)
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
               else
                    {
                    _context.AuthorGenres.Add(current);
                    }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }
        public async Task<IActionResult> AddAuthor(string authorName)
        {
            var author = new Author();
            author.Name = authorName;
            //author.Details = description;
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var id = (await _context.Authors.FirstAsync(x => x.Name.Equals(authorName))).AuthorId;
            //var id = db.Location.First(x => x.Address == locationName).Id;

            return Ok(new
            {
                Id = id,
                Name = authorName,
            });
        }

        public async Task<IActionResult> AddGenre(string genreName)
        {
            var genre = new Genre();
            genre.Name = genreName;
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            var id = (await _context.Genres.FirstAsync(x => x.Name.Equals(genreName))).GenreId;
            //var id = db.Location.First(x => x.Address == locationName).Id;

            return Ok(new
            {
                Id = id,
                Name = genreName,
            });
        }
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(a => a.Author)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.BookId == id);
           
            book.AuthorList = (ICollection<SelectListItem>)CreateAuthorDropDownList();
            book.GenreList = (ICollection<SelectListItem>)CreateGenreDropDownList();
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("BookId,Name,Description,AuthorId,GenreId")] Book book)
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Name,Description, AuthorId, GenreId")] Book book,
            [Bind("Name")] Author author, [Bind("Name")] Genre genre)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CreateAuthorDropDownList();
                    CreateGenreDropDownList();
                    //book.Author = await _context.Authors.FirstAsync(a => a.Name.Equals(author.Name));
                    //book.AuthorId = author.AuthorId;
                    //book.Genre = await _context.Genres.FirstAsync(a => a.Name.Equals(genre.Name));
                    //book.GenreId = book.Genre.GenreId;
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(a => a.Author)
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(m => m.BookId == id);
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
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        private async Task<bool> AuthorgenresExistsAsync(AuthorGenres record)
        {
            
            if ((await _context.AuthorGenres.FindAsync(record.AuthorId)) != null)

                if ((await _context.AuthorGenres.FindAsync(record.GenreId)) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
           
            else
            {
                return false;
            }
          
        }

        public IEnumerable<SelectListItem> CreateAuthorDropDownList()
        {
            var book = new Book();
            book.AuthorList = _context.Authors
                                  .Select(a => new SelectListItem()
                                  {
                                      Value = a.AuthorId.ToString(),
                                      Text = a.Name
                                  })
                                          .ToList();

            return book.AuthorList;

        }
        public IEnumerable<SelectListItem> CreateGenreDropDownList()
        {
            var book = new Book();
            book.GenreList = _context.Genres
                                  .Select(a => new SelectListItem()
                                  {
                                      Value = a.GenreId.ToString(),
                                      Text = a.Name
                                  })
                                          .ToList();
            return book.GenreList;
        }


    }
    /*public IEnumerable<SelectListItem> CreateAuthorDropDownList()
    {
        var book = new Book();
        book.AuthorList = new List<SelectListItem>();
        book.AuthorList.Add(new SelectListItem
        {
            Value = "",
            Text = "Select Author"
        });
        foreach (var item in _context.Authors)
        {
            book.AuthorList.Add(new SelectListItem
            {
                Value = Convert.ToString(item.AuthorId),

                Text = item.Name
            });
        }


        return book.AuthorList;
    }*/

}
