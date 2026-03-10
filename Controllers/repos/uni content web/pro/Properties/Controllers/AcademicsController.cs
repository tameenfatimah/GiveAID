using Microsoft.AspNetCore.Mvc;

namespace pro.Properties.Controllers
{
    public class AcademicsController : Controller
    {
        public IActionResult Undergraduate()
        {
            return View();
        }
        public IActionResult Postgraduate()
        {
            return View();
        }
        public IActionResult Admission()
        {
            return View();
        }
    }
}
