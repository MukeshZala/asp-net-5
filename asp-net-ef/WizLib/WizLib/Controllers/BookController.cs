using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            /*
            List<Book> books= _db.Books.ToList<Book>();
            foreach (var item in books)
            {
                // it will call DB for each publisher even though it's distinct. 
                //item.Publisher = _db.Publishers.FirstOrDefault(p => p.Publisher_Id == item.Publisher_Id); 
                //explicit loading will use distinct publishers only . 
                _db.Entry(item).Reference(u => u.Publisher).Load(); 
            }
            */

            //Eager loading, it will make inner join in EF query 
            List<Book> books = _db.Books.Include(u => u.Publisher).ToList(); 

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
                obj.Book = _db.Books.Include(b=> b.BookDetail).FirstOrDefault(o => o.Book_Id == Id);
                if (obj.Book == null)
                    return NotFound();

              
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


        public IActionResult PlayGround()
        {
            AttachVsUpdate();

            //Performance
            CheckPerformanceOfQuery();

            //Eagerloading 
            EagerLoading(); 


            return RedirectToAction(nameof(Index));

        }

        private void AttachVsUpdate()
        {
            //No change in book entity but change in bookdetail, Update will execute 2 queries to DB 1 for Book, 2 - book detail. 
            var book1 = _db.Books.Include(a=> a.BookDetail).FirstOrDefault(o => o.Book_Id == 3);
            book1.BookDetail.NumberOfPages = 500;
            _db.Books.Update(book1);
            _db.SaveChanges();


            var book2 = _db.Books.Include(a => a.BookDetail).FirstOrDefault(o => o.Book_Id == 3);
            book2.BookDetail.Weight = 25.89; 
            _db.Books.Attach(book2);
            _db.SaveChanges();


        }

        private void EagerLoading()
        {
            //executed immediately after below statement 
            var bookTemp = _db.Books.FirstOrDefault();
            bookTemp.Price = 100;

            //not executed immediately 
            var bookCollection = _db.Books;
            double totalPrice = 0;

            //db operation happen when IEnumerable object will be used. 
            foreach (var book in bookCollection)
            {
                totalPrice += book.Price;
            }

            //db operation happens since the object needs to convert into ToList/ToArray etc. 
            var bookList = _db.Books.ToList();
            foreach (var book in bookList)
            {
                totalPrice += book.Price;
            }

            //No db query. 
            var bookCollection2 = _db.Books;
            //db query since need count of object. 
            var bookCount1 = bookCollection2.Count();

            //immediate db query since we need count value immmediately. 
            var bookCount2 = _db.Books.Count();
        }

        private void CheckPerformanceOfQuery()
        {
            IEnumerable<Book> books = _db.Books;
            var filteredBooks1 = books.Where(b => b.Price > 10).ToList();


            IQueryable<Book> queryBooks = _db.Books;
            var filter2 = queryBooks.Where(b => b.Price > 10).ToList();

            int i = 0;
        }
    }
}
