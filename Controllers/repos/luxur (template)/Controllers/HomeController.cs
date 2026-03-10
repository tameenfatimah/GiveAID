using luxur.Database;
using luxur.Models;
using Microsoft.AspNetCore.Mvc;

namespace luxur.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;
            public HomeController(AppDbContext context)
            {
                this.context = context;
            }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string fname,string lname,string email,string message)
        {
            Contact con = new Contact()
            {
                Fname = fname,
                Lname = lname,
                Email = email,
                Message = message
            };
            context.Contacts.Add(con);
            context.SaveChanges();
            return View();
        }
    }
}
