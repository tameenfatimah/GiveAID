using Microsoft.AspNetCore.Mvc;

namespace Institute.Controllers
{
    public class LibraryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
