using Microsoft.AspNetCore.Mvc;

namespace pro.Properties.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
