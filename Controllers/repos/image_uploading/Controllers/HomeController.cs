using image_uploading.Models;
using Microsoft.AspNetCore.Mvc;

namespace image_uploading.Controllers
{
    public class HomeController : Controller
    {
        private readonly Mydbcontext context;
        private readonly IWebHostEnvironment env;

        public HomeController(Mydbcontext context , IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string txtname,IFormFile txtimage, Product pro)
        {
            string filename = Path.GetFileName(txtimage.FileName);
            string filepath = Path.Combine(env.WebRootPath, "Product_Image",filename);

            FileStream fs = new FileStream(filepath,FileMode.Create);
            txtimage.CopyTo(fs);

            pro.Name = txtname;
            pro.Image = filename;

            context.Products.Add(pro);
            context.SaveChanges();
            return View();
        }
    }
}
