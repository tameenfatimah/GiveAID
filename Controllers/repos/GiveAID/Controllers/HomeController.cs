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
            /* Fetching the latest 3 upcoming events, and the latest
             6 gallery items to display on the homepage*/
            var upcomingEvents = _db.Events.Where(e=> e.IsUpcoming == true)
                                           .OrderByDescending(e => e.EventDate)
                                           .Take(3)  // Shows only the latest 3 upcoming events
                                           .ToList();
            
            var galleryPreview = _db.Galleries.OrderByDescending(g=>g.UploadedAt)
                                              .Take(6)  // Shows only the latest 6 gallery items
                                              .ToList();

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
        public IActionResult Partners()
        {
            var partners = _db.Partners.Where(p => p.IsActive == true)
                                       .ToList();
            return View(partners);
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string fullName, string email, string subject, string message)
        {
            var contact = new ContactMessage
            {
                FullName = fullName,
                guestEmail = email,
                Subject = subject,
                Message = message,
                SubmittedAt = DateTime.Now,
                IsRead = false
            };

            _db.ContactMessages.Add(contact);
            _db.SaveChanges();

            ViewBag.SuccessMessage = "Thank you for contacting us! We will get back to you soon.";
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
            var upcomingEvents = _db.Events.Where(e => e.IsUpcoming == true)
                                       .OrderBy(e => e.EventDate)
                                       .ToList();

            var pastEvents = _db.Events.Where(e => e.IsUpcoming == false)
                                       .OrderByDescending(e => e.EventDate)
                                       .ToList();

            ViewBag.UpcomingEvents = upcomingEvents;
            ViewBag.PastEvents = pastEvents;
            return View();
        }
        public IActionResult EventDetail(int id)
        {
            var eventItem = _db.Events.FirstOrDefault(e => e.EventId == id);
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
        public IActionResult HelpCentre (string subject, string message)
        {
            // Check if user is logged in via Session
            var UserId = HttpContext.Session.GetInt32("UserId");

            if (UserId == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Build a new Query object and save it
            var query = new Query
            {
                UserId = UserId.Value,
                Subject = subject,
                Message = message,
                IsRepliedAt = false,
                SubmittedAt = DateTime.Now
            };

            _db.Queries.Add(query);
            _db.SaveChanges();

            // Shows a success message
            ViewBag.SuccessMessage = "Your query has been submitted! We will reply shortly.";
            return View();
        }
    }
}
