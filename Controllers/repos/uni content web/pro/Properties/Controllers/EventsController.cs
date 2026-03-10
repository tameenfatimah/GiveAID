using Microsoft.AspNetCore.Mvc;

namespace pro.Properties.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Events()
        {
            return View();
        }
    }
}
