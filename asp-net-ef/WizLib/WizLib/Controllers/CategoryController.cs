using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WizLib_DataAccess.Data;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> categories = _db.Category.ToList();
         return View(categories); 
        }

        public IActionResult Upsert(int ? Id)
        {
            Category obj = new Category();
            if ( Id != null )
            {
                obj = _db.Category.FirstOrDefault(u => u.Category_Id == Id);

                if (obj == null)
                    return NotFound(); 

            }


            return View(obj); 
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj )
        {
            if (  ModelState.IsValid)
            {
                if ( obj.Category_Id == 0)
                {
                    _db.Category.Add(obj);
                }else
                {
                    _db.Category.Update(obj);
                }

                _db.SaveChanges();
                return RedirectToAction(nameof(Index)); 

            }
            return View(obj); 
        }

        public IActionResult Delete(int Id)
        {
            var catFromDb = _db.Category.FirstOrDefault(u => u.Category_Id.Equals(Id));
            _db.Category.Remove(catFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

    }
}
