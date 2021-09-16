using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            List<Book> books= _db.Books.ToList<Book>();
            return View(books);
        }
        /*
        public IActionResult Upsert(int? Id)
        {
            Author author = new Author();
            if (Id != null)
            {
                author = _db.Authors.FirstOrDefault<Author>(u => u.Author_Id.Equals(Id));
                if (author == null)
                    return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Author author)
        {
            if (ModelState.IsValid)
            {
                if (author.Author_Id == 0)
                    _db.Authors.Add(author);
                else
                    _db.Authors.Update(author);

                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
                return View(author);
        }

        public IActionResult Delete(int Id)
        {
            var author = _db.Authors.FirstOrDefault<Author>(u => u.Author_Id == Id);
            if (author == null)
                return NotFound();
            else
                _db.Authors.Remove(author);

            _db.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
        */

    }
}
