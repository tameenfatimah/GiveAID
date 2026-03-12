using GiveAID.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
namespace GiveAID.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register(string name, string email, string password, string confirmPassword, string phone,string address, string profession)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone))
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View();
            }
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }
            if (password.Length < 6)
            {
                ViewBag.Error = "Password must be at least 6 characters.";
                return View();
            }
            User existingUser = _db.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.Error = "This email is already registered!";
                return View();
            }
            User newUser = new User();
            newUser.Name = name;
            newUser.Email = email;
            newUser.Password = password;
            newUser.Phone = phone;
            newUser.Address = address;
            newUser.Profession = profession;
            newUser.Role = "User";
            newUser.RegisteredAt = DateTime.Now;

            _db.Users.Add(newUser);
            _db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("UserName", newUser.Name);
            HttpContext.Session.SetString("UserRole", newUser.Role);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please enter email and password.";
                return View();
            }
            User user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserRole", user.Role);
            if (user.Role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            User user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var donations = _db.Donations.Where(d => d.UserId == userId.Value).OrderByDescending(d => d.DonationDate).ToList();
            ViewBag.Donations = donations;
            return View(user);
        }
        public IActionResult EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            User user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
            return View(user);
        }
        [HttpPost]
        public IActionResult EditProfile(string name,string phone,string address,string profession)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            User user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
            {
                ViewBag.Error = "Name and Phone cannot be empty.";
                return View(user);
            }
            user.Name = name;
            user.Phone = phone;
            user.Address = address;
            user.Profession = profession;

            _db.SaveChanges();

            HttpContext.Session.SetString("UserName", user.Name);
            ViewBag.Success = "Profile updated!";
            return View(user);
        }
        public IActionResult ChangePassword()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string currentPassword,string newPassword,string confirmNewPassword)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            User user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            if (user.Password != currentPassword)
            {
                ViewBag.Error = "Current password is incorrect.";
                return View();
            }
            if (newPassword != confirmNewPassword)
            {
                ViewBag.Error = "New passwords do not match.";
                return View();
            }
            if (newPassword.Length < 6)
            {
                ViewBag.Error = "Password must be at least 6 characters.";
                return View();
            }

            user.Password = newPassword;
            _db.SaveChanges();

            ViewBag.Success = "Password changed.";
            return View();
        }
        public IActionResult InviteFriend()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public IActionResult InviteFriend(string friendEmail, string friendName)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            string senderName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrWhiteSpace(friendEmail))
            {
                ViewBag.Error = "Please enter your friend's email!";
                return View();
            }
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

                mail.From = new MailAddress("giveaidproject2026@gmail.com");
                mail.To.Add(friendEmail);
                mail.Subject = senderName + " invited you to join Give-AID!";
                mail.Body = "Hi " + friendName + ",\n\n" + senderName + " wants you to join Give-AID.\n\n" + "Visit: http://www.giveaid.com\n\n" + "Give-AID Team";

                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(
                    "giveaidproject2026@gmail.com",
                    "ioec wdpr mfji jwgk"
                );

                client.Send(mail);
                ViewBag.Success = "Invitation sent to " + friendEmail;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Could not send email: " + ex.Message;
            }
            return View();
        }
        public IActionResult MyQueries()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var queries = _db.Queries.Where(q => q.UserId == userId.Value).OrderByDescending(q => q.SubmittedAt).ToList();
            return View(queries);
        }
    }
}