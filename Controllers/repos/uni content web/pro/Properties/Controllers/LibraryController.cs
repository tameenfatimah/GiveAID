using Microsoft.AspNetCore.Mvc;

namespace pro.Properties.Controllers
{
    public class LibraryController : Controller
    {
        public IActionResult Library()
        {
            return View();
        }
    }
}
