using GiveAID.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var upcomingEvents = _db.Events.Where(e => e.IsUpcoming == true).OrderByDescending(e => e.EventDate).Take(3).ToList();
            var galleryPreview = _db.Galleries.OrderByDescending(g => g.UploadedAt).Take(6).ToList();

            ViewBag.Events = upcomingEvents;
            ViewBag.GalleryPreview = galleryPreview;
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult WhatWeDo()
        {
            return View();
        }
        public IActionResult OurMission()
        {
            return View();
        }
        public IActionResult OurTeam()
        {
            return View();
        }
        public IActionResult Career()
        {
            return View();
        }
        public IActionResult Achievements()
        {
            return View();
        }
        public IActionResult OurSupporters()
        {
            return View();
        }
        public IActionResult NGOs()
        {
            var ngos = _db.NGOs.Where(n => n.IsActive == true).OrderBy(n => n.Name).ToList();
            return View(ngos);
        }
        public IActionResult Partners()
        {
            var partners = _db.Partners.Where(p => p.IsActive == true).ToList();
            return View(partners);
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string fullName, string email, string subject, string message)
        {
            ContactMessage contact = new ContactMessage();
            contact.FullName = fullName;
            contact.guestEmail = email;
            contact.Subject = subject;
            contact.Message = message;
            contact.SubmittedAt = DateTime.Now;
            contact.IsRead = false;

            _db.ContactMessages.Add(contact);
            _db.SaveChanges();

            ViewBag.SuccessMessage = "Your message has been sent successfully!";
            return View();
        }
        public IActionResult Gallery(string category = null)
        {
            var query = _db.Galleries.AsQueryable();
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(g => g.Category == category);
            }
            var images = query.OrderByDescending(g => g.UploadedAt).ToList();
            ViewBag.SelectedCategory = category;
            return View(images);
        }
        public IActionResult Events()
        {
            var events = _db.Events.OrderByDescending(e => e.EventDate).ToList();
            return View(events);
        }
        public IActionResult EventDetail(int id)
        {
            Event? eventItem = _db.Events.FirstOrDefault(e => e.EventId == id);
            if (eventItem == null)
            {
                return RedirectToAction("Events");
            }
            return View(eventItem);
        }
        public IActionResult HelpCentre()
        {
            return View();
        }
        [HttpPost]
        public IActionResult HelpCentre(string subject, string message)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            Query query = new Query();
            query.UserId = userId.Value;
            query.Subject = subject;
            query.Message = message;
            query.IsRepliedAt = false;
            query.SubmittedAt = DateTime.Now;

            _db.Queries.Add(query);
            _db.SaveChanges();

            ViewBag.SuccessMessage = "Your query has been submitted successfully.";
            return View();
        }
    }
}