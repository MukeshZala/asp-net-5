using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;
using WizLib_Model.ViewModels;

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
            foreach (var item in books)
            {
                // it will call DB for each publisher even though it's distinct. 
                //item.Publisher = _db.Publishers.FirstOrDefault(p => p.Publisher_Id == item.Publisher_Id); 
                //explicit loading will use distinct publishers only . 
                _db.Entry(item).Reference(u => u.Publisher).Load(); 
            }
            return View(books);
        }
        
        public IActionResult Upsert(int? Id)
        {
            BookVM bookVM = new BookVM();

            bookVM.PublisherList = _db.Publishers.Select(
                    obj => new SelectListItem()
                    {
                        Text = obj.Name,
                        Value = obj.Publisher_Id.ToString()
                    }
                );




            if (Id != null)
            {
                bookVM.Book = _db.Books.FirstOrDefault(o => o.Book_Id == Id);
                if (bookVM.Book == null)
                    return NotFound(); 

            }

            return View(bookVM);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM bookVM)
        {
            if (bookVM.Book.Book_Id == 0)
            {
                _db.Books.Add(bookVM.Book);
            }
            else
                _db.Books.Update(bookVM.Book);

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            var dbBook = _db.Books.FirstOrDefault(b => b.Book_Id == Id);
            if (dbBook == null)
                return NotFound();
            else
                _db.Books.Remove(dbBook); 

            _db.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? Id)
        {
            BookVM obj = new BookVM();

            if (Id  == null)
            {
                obj.Book.BookDetail = new BookDetail();
            }
            else
            {
                obj.Book = _db.Books.FirstOrDefault(o => o.Book_Id == Id);
                if (obj.Book == null)
                    return NotFound();

                obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(o => o.BookDetail_Id == obj.Book.BookDetail_Id); 
            }


            return View(obj); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj )
        {
            if ( obj.Book.BookDetail.BookDetail_Id  ==0 )
            {
                _db.BookDetails.Add(obj.Book.BookDetail);
                _db.SaveChanges();

                Book book = _db.Books.FirstOrDefault(b => b.Book_Id == obj.Book.Book_Id);
                book.BookDetail_Id = obj.Book.BookDetail.BookDetail_Id;
                _db.SaveChanges();
            }else
            {
                _db.BookDetails.Update(obj.Book.BookDetail);
                _db.SaveChanges(); 
            }
            
            return RedirectToAction(nameof(Index));
        }

    }
}
