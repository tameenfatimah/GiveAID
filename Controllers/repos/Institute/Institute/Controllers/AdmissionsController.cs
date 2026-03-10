using Microsoft.AspNetCore.Mvc;

namespace Institute.Controllers
{
    public class AdmissionsController : Controller
    {
        public IActionResult Undergraduate()
        {
            return View();
        }
        public IActionResult Postgraduate()
        {
            return View();
        }
    }
}
