using Login_Register.Data;
using Login_Register.Models;
using Microsoft.AspNetCore.Mvc;

namespace Login_Register.Controllers
{
    public class HomeController : Controller
    {
        private readonly Mydbcontext context;

        public HomeController(Mydbcontext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
             if (HttpContext.Session.GetString("UserEmail") == null)
                {
                    return RedirectToAction("Login");
                }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Users user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Users user)
        {
            var login = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (login != null)
            {
                HttpContext.Session.SetString("UserEmail", login.Email);
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Invalid Email or Password";
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            ViewBag.Message = "You've been logged out!";
            return View();
        }

    }
}
