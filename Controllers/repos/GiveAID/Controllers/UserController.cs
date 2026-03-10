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
            //If User has already logged in, redirect to home page (using if statement to check if user is authenticated)
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register(string name, string email, string password,
                                      string confirmPassword, string phone,
                                      string address, string profession)
        {
            // Check if any field is empty
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(phone))
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View();
            }
            // Check if passwords match
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match. Please try again.";
                return View();
            }
            // Check if password is at least 6 characters
            if (password.Length < 6)
            {
                ViewBag.Error = "Password must be at least 6 characters long.";
                return View();
            }
            // Check if this email is already registered
            var existingUser = _db.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.Error = "This email is already registered. Please login instead.";
                return View();
            }
            //Saving to the database
            var newUser = new User
            {
                Name = name,
                Email = email,
                Password = password,   
                Phone = phone,
                Address = address,
                Profession = profession,
                Role = "User",      // All new registrations are regular Users
                RegisteredAt = DateTime.Now
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            //Auto-login after registration
            // After saving, log them in immediately
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("UserName", newUser.Name);
            HttpContext.Session.SetString("UserRole", newUser.Role);

            // Send them to homepage after successful registration
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            // If already logged in, redirect to homepage
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Validation: Check if email and password are provided
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please enter both email and password.";
                return View();
            }
            // Checking credentials against the database
            // Find user with matching email & password
            var user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                // No user found — wrong email or password
                ViewBag.Error = "Invalid email or password. Please try again.";
                return View();
            }
            // Creating session for the logged-in user
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserRole", user.Role);

            // This code redirects users based on their role after login
            // Admins go to Admin Dashboard
            // Regular users go to Homepage
            if (user.Role == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Logout()
        {
            // Clear everything stored in session
            HttpContext.Session.Clear();
            // Sends them back to homepage
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Profile()
        {
            // Get logged-in user's ID from session
            var userId = HttpContext.Session.GetInt32("UserId");

            // If not logged in, send to login page
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            // Fetch full user data from database
            var user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            // Also fetch this user's donation history
            var donations = _db.Donations
                                .Where(d => d.UserId == userId.Value)
                                .OrderByDescending(d => d.DonationDate)
                                .ToList();
            ViewBag.Donations = donations;
            // Pass user object to the view
            return View(user);
        }
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
            return View(user);
        }
        [HttpPost]
        public IActionResult EditProfile(string name, string phone,string address, string profession)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            // Find this user in the database
            var user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            // Validation: Name and Phone cannot be empty
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
            {
                ViewBag.Error = "Name and Phone cannot be empty.";
                return View(user);
            }
            // Update the fields
            user.Name = name;
            user.Phone = phone;
            user.Address = address;
            user.Profession = profession;
            // Save changes to database
            _db.SaveChanges();
            // Update the name in session too so navbar shows new name
            HttpContext.Session.SetString("UserName", user.Name);
            ViewBag.Success = "Profile updated successfully!";
            return View(user);
        }
        public IActionResult ChangePassword()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string currentPassword,
                                             string newPassword,
                                             string confirmNewPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var user = _db.Users.FirstOrDefault(u => u.UserId == userId.Value);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            // Check current password is correct
            if (user.Password != currentPassword)
            {
                ViewBag.Error = "Current password is incorrect.";
                return View();
            }
            // Check new passwords match
            if (newPassword != confirmNewPassword)
            {
                ViewBag.Error = "New passwords do not match.";
                return View();
            }
            // Check new password length
            if (newPassword.Length < 6)
            {
                ViewBag.Error = "New password must be at least 6 characters.";
                return View();
            }
            // Update password
            user.Password = newPassword;
            _db.SaveChanges();
            ViewBag.Success = "Password changed successfully!";
            return View();
        }
        public IActionResult InviteFriend()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public IActionResult InviteFriend(string friendEmail, string friendName)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var senderName = HttpContext.Session.GetString("UserName");
            // Validate email
            if (string.IsNullOrWhiteSpace(friendEmail))
            {
                ViewBag.Error = "Please enter your friend's email address.";
                return View();
            }
            try
            {
                // Building the email
                var mail = new MailMessage();
                var client = new SmtpClient("smtp.gmail.com", 587);
                mail.From = new MailAddress("yourgmail@gmail.com");  // ← put your Gmail here
                mail.To.Add(friendEmail);
                mail.Subject = senderName + " invited you to join Give-AID!";
                mail.Body = "Hi " + friendName + ",\n\n"
                             + senderName + " thinks you should join Give-AID — "
                             + "a platform that brings together NGOs to help those in need.\n\n"
                             + "Visit us and make a difference: http://www.giveaid.com\n\n"
                             + "Best regards,\nThe Give-AID Team";

                // SMPT (Simple Mail Transfer Protocol) settings for Gmail
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(
                    "yourgmail@gmail.com",     // ← your Gmail
                    "your-app-password"        // ← Gmail App Password (NOT your real password)
                );
                client.Send(mail);
                ViewBag.Success = "Invitation sent successfully to " + friendEmail + "!";
            }
            catch (Exception ex)
            {
                // If email fails, show error but don't crash
                ViewBag.Error = "Could not send email. Error: " + ex.Message;
            }
            return View();
        }
        public IActionResult MyQueries()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var queries = _db.Queries
                              .Where(q => q.UserId == userId.Value)
                              .OrderByDescending(q => q.SubmittedAt)
                              .ToList();
            return View(queries);
        }
    }
}
