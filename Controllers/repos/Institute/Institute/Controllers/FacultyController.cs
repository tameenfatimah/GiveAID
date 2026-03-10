using Microsoft.AspNetCore.Mvc;

namespace Institute.Controllers
{
    public class FacultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
