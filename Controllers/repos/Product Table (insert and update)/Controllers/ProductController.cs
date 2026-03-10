using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Project2.Data;
using Project2.Models;

namespace Project2.Controllers
{
    public class ProductController : Controller
    {
        private readonly Mydbcontext context;

        public ProductController(Mydbcontext context)
        {
            this.context = context;
        }
        public IActionResult Insert()
        {
            return View();
        }
        public IActionResult Index()
        {
            var products = new List<Product>(context.products.ToList());
            return View(products);
        }
        [HttpPost]
        public IActionResult Insert(string txtname,string txtdesc)
        {
            Product pro = new Product()
            {
                Name = txtname,
                Description = txtdesc,
            };
            context.products.Add(pro);
            context.SaveChanges();
            TempData["Message"] = "Product Added";
            return RedirectToAction("Index");
        }
        //Update Product
        public IActionResult Edit(int proid)
        {
            Product? products = context.products.Find(proid);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        [HttpPost]
        public IActionResult Edit(int proid, string txtname, string txtdesc)
        {
            Product? prod = context.products.Find(proid);
            if (prod == null)
            {
                return NotFound();
            }
            prod.Name = txtname;
            prod.Description = txtdesc;

            context.products.Update(prod);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
