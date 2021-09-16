using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PublisherController(ApplicationDbContext db )
        {
            _db = db; 
        }

        public IActionResult Index()
        {
            List<Publisher> publishers = _db.Publishers.ToList<Publisher>(); 


            return View(publishers);
        }

        public IActionResult Upsert(int? Id)
        {
            Publisher publisher = new Publisher(); 
            if ( Id != null)
            {
                publisher = _db.Publishers.FirstOrDefault<Publisher>(u => u.Publisher_Id == Id);
                if (publisher == null)
                    return NotFound(); 

            }

            return View(publisher); 

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                if (publisher.Publisher_Id == 0)
                    _db.Publishers.Add(publisher);
                else
                {
                    _db.Publishers.Update(publisher);
                }

                _db.SaveChanges();
                return RedirectToAction(nameof(Index)); 
            }
            else
                return View(publisher); 

        }

        public IActionResult Delete(int Id)
        {
            var publisher = _db.Publishers.FirstOrDefault<Publisher>(u => u.Publisher_Id.Equals(Id));
            if (publisher == null)
                return NotFound();
            else
                _db.Publishers.Remove(publisher);

            _db.SaveChanges(); 

            return RedirectToAction(nameof(Index)); 
        }
    }
}
