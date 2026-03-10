using Microsoft.AspNetCore.Mvc;

namespace pro.Properties.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult News()
        {
            return View();
        }
    }
}
