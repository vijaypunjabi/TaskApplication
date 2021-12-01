using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskApplication.InfraForAuthentication;
using TaskApplication.Models;
using PagedList;
using PagedList.Mvc;

namespace TaskApplication.Controllers
{
    [CustomAuthenticationFilter]
    public class CategoriesController : Controller
    {
        private TaskContext db = new TaskContext();

        // GET: Categories
        public async Task<ActionResult> Index(int page = 1 , int pageSize = 10)
        {
            //For Pagination
            var listCategories = await db.Categories.ToListAsync();
            PagedList<Category> categories = new PagedList<Category>(listCategories, page, pageSize);
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        [CustomAuthorizationFilter("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> Create(/*[Bind(Include = "Id,Name,CreatedBy")]*/ Category category)
        {
            if (ModelState.IsValid) 
            {
                //db.Categories.Add(category);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
                Category _category = new Category();
                _category.Name = category.Name;
                _category.IsActive = category.IsActive;
                _category.CreatedBy = category.CreatedBy;
                _category.CreatedDate = DateTime.Now;
                db.Categories.Add(_category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");


            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CreatedBy")] Category category)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(category).State = EntityState.Modified;
                //if(category.IsActive == false)
                //{
                //    var products = new List<Product>();
                //    products = await db.Products.Where(p => p.CategoryId == category.Id).ToListAsync();
                //    foreach(var p in products)
                //    {
                //        p.IsActive = false;
                //    }
                //}
                //else
                //{
                //    var products = new List<Product>();
                //    products = await db.Products.Where(p => p.CategoryId == category.Id).ToListAsync();
                //    foreach(var p in products)
                //    {
                //        p.IsActive = true;
                //    }
                Category _category = db.Categories.Where(u => u.Id == category.Id).SingleOrDefault();
                _category.Name = category.Name;
                _category.IsActive = category.IsActive;
                _category.CreatedBy = category.CreatedBy;
                _category.CreatedDate = DateTime.Now;
                db.Entry(_category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
                await db.SaveChangesAsync();
             //   return RedirectToAction("Index"); }
            
            return View(category);
        }

        // GET: Categories/Delete/5
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
