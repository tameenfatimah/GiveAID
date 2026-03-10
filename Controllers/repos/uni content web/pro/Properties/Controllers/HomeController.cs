using Microsoft.AspNetCore.Mvc;

namespace pro.Properties.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
