using Microsoft.AspNetCore.Mvc;

namespace pro.Properties.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}
