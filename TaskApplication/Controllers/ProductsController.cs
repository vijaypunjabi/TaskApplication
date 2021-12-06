using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using TaskApplication.InfraForAuthentication;
using TaskApplication.Models;

namespace TaskApplication.Controllers
{
    [CustomAuthenticationFilter]
    public class ProductsController : Controller
    {
        private TaskContext db = new TaskContext();

        // GET: Products
        [CustomAuthorizationFilter("Admin", "Normal")]
        public async Task<ActionResult> Index(int page = 1 , int pageSize = 10)
        {
            var userName = Convert.ToString(HttpContext.Session["UserName"]);
            var IsAdmin = (from u in db.Users join r in db.Roles on u.RoleId equals r.Id where u.UserName == userName select r.Name).FirstOrDefault();
            if (IsAdmin == "Admin")
            {
                ViewBag.Show = true;
            }
            //var products = db.Products.Include(p => p.Category);
            //return View(await products.ToListAsync());
            var listProducts = await db.Products.ToListAsync();
            PagedList<Product> product = new PagedList<Product>(listProducts, page, pageSize);
            return View(product);
        }

        // GET: Products/Details/5
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [CustomAuthorizationFilter("Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name" ,"CreatedBy");
            return View();
        }

        // POST: Products/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizationFilter("Admin")]
        public async Task <ActionResult> Create([Bind(Include = "Id,Name,CreatedBy,CategoryId,IsActive")] Product product)
        {
            if (ModelState.IsValid)
            {
                var addProduct = new Product();
                addProduct.Name = product.Name;
                addProduct.CategoryId = product.CategoryId;
                addProduct.IsActive = product.IsActive;
                addProduct.CreatedBy = (int) Session["UserId"];
                addProduct.CreatedDate = DateTime.Now;
             
                

                db.Products.Add(addProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name" , "CreatedBy", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizationFilter("Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedBy,CategoryId,IsActive")] Product product)
        {
            if(ModelState.IsValid)
            {

                var userId = Session["UserId"];
                var addProduct = db.Products.Where(u => u.Id == product.Id).SingleOrDefault();
                addProduct.Name = product.Name;
                addProduct.CategoryId = product.CategoryId;
                addProduct.IsActive = product.IsActive;
                addProduct.CreatedBy = (int)userId;
                addProduct.CreatedDate = DateTime.Now;

                db.Entry(addProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

            //if (ModelState.IsValid)
            //{
            //    db.Entry(product).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            //return View(product);
        }

        // GET: Products/Delete/5
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorizationFilter("Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            db.SaveChanges();
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
