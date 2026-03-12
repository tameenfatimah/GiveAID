using GiveAID.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GiveAID.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }
        public IActionResult Dashboard()
        {
            ViewBag.RecentDonations = _db.Donations.OrderByDescending(d => d.DonationDate).Take(5).ToList();

            ViewBag.Users = _db.Users.OrderByDescending(u => u.RegisteredAt).ToList();
            ViewBag.NGOs = _db.NGOs.OrderBy(n => n.Name).ToList();
            ViewBag.Events = _db.Events.OrderByDescending(e => e.EventDate).ToList();
            ViewBag.Partners = _db.Partners.OrderBy(p => p.CompanyName).ToList();
            ViewBag.Donations = _db.Donations.OrderByDescending(d => d.DonationDate).ToList();
            ViewBag.Queries = _db.Queries.OrderBy(q => q.IsRepliedAt).ThenByDescending(q => q.SubmittedAt).ToList();
            ViewBag.Messages = _db.ContactMessages.OrderBy(c => c.IsRead).ThenByDescending(c => c.SubmittedAt).ToList();
            ViewBag.Gallery = _db.Galleries.OrderByDescending(g => g.UploadedAt).ToList();

            return View();
        }
        public IActionResult ManageUsers()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var users = _db.Users.OrderByDescending(u => u.RegisteredAt).ToList();
            return View(users);
        }
        public IActionResult DeleteUser(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            User? user = _db.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("ManageUsers");
            }
            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (user.UserId == currentUserId)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction("ManageUsers");
            }
            _db.Users.Remove(user);
            _db.SaveChanges();
            TempData["Success"] = "User deleted.";
            return RedirectToAction("ManageUsers");
        }
        public IActionResult ManageNGOs()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var ngos = _db.NGOs.OrderBy(n => n.Name).ToList();
            return View(ngos);
        }
        public IActionResult AddNGO()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            return View();
        }
        [HttpPost]
        public IActionResult AddNGO(string name, string description, string contactEmail, string contactPhone, string website, IFormFile logoFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "NGO name is required.";
                return View();
            }
            string logoPath = "/images/default-ngo.png";
            if (logoFile != null && logoFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    logoFile.CopyTo(stream);
                }
                logoPath = "/images/" + fileName;
            }
            NGO ngo = new NGO();
            ngo.Name = name;
            ngo.Description = description;
            ngo.ContactEmail = contactEmail;
            ngo.ContactPhone = contactPhone;
            ngo.Website = website;
            ngo.LogoPath = logoPath;
            ngo.AddedOn = DateTime.Now;
            ngo.IsActive = true;

            _db.NGOs.Add(ngo);
            _db.SaveChanges();

            TempData["Success"] = "NGO added.";
            return RedirectToAction("ManageNGOs");
        }
        public IActionResult EditNGO(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            NGO? ngo = _db.NGOs.FirstOrDefault(n => n.NGOId == id);
            if (ngo == null) return RedirectToAction("ManageNGOs");
            return View(ngo);
        }
        [HttpPost]
        public IActionResult EditNGO(int id, string name, string description, string contactEmail, string contactPhone, string website, bool isActive, IFormFile logoFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            NGO? ngo = _db.NGOs.FirstOrDefault(n => n.NGOId == id);
            if (ngo == null) return RedirectToAction("ManageNGOs");
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "NGO name is required.";
                return View(ngo);
            }
            if (logoFile != null && logoFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    logoFile.CopyTo(stream);
                }
                ngo.LogoPath = "/images/" + fileName;
            }
            ngo.Name = name;
            ngo.Description = description;
            ngo.ContactEmail = contactEmail;
            ngo.ContactPhone = contactPhone;
            ngo.Website = website;
            ngo.IsActive = isActive;

            _db.SaveChanges();
            TempData["Success"] = "NGO updated.";
            return RedirectToAction("ManageNGOs");
        }
        public IActionResult DeleteNGO(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            NGO? ngo = _db.NGOs.FirstOrDefault(n => n.NGOId == id);
            if (ngo != null)
            {
                _db.NGOs.Remove(ngo);
                _db.SaveChanges();
                TempData["Success"] = "NGO deleted.";
            }
            return RedirectToAction("ManageNGOs");
        }
        public IActionResult ManageGallery()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var images = _db.Galleries.OrderByDescending(g => g.UploadedAt).ToList();
            return View(images);
        }
        public IActionResult UploadImage()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            return View();
        }
        [HttpPost]
        public IActionResult UploadImage(string caption, string category, IFormFile imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            if (imageFile == null || imageFile.Length == 0)
            {
                ViewBag.Error = "Please select an image.";
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            Gallery gallery = new Gallery();
            gallery.ImagePath = "/images/" + fileName;
            gallery.Caption = caption;
            gallery.Category = category;
            gallery.UploadedAt = DateTime.Now;

            _db.Galleries.Add(gallery);
            _db.SaveChanges();

            TempData["Success"] = "Image uploaded.";
            return RedirectToAction("ManageGallery");
        }
        public IActionResult DeleteImage(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Gallery? image = _db.Galleries.FirstOrDefault(g => g.GalleryId == id);
            if (image != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + image.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _db.Galleries.Remove(image);
                _db.SaveChanges();
                TempData["Success"] = "Image deleted.";
            }
            return RedirectToAction("ManageGallery");
        }
        public IActionResult ManagePartners()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var partners = _db.Partners.OrderBy(p => p.CompanyName).ToList();
            return View(partners);
        }
        public IActionResult AddPartner()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            return View();
        }
        [HttpPost]
        public IActionResult AddPartner(string companyName, string description, string website, IFormFile logoFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            if (string.IsNullOrWhiteSpace(companyName))
            {
                ViewBag.Error = "Company name is required.";
                return View();
            }
            string logoPath = "/images/default-partner.png";
            if (logoFile != null && logoFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    logoFile.CopyTo(stream);
                }
                logoPath = "/images/" + fileName;
            }
            Partner partner = new Partner();
            partner.CompanyName = companyName;
            partner.Description = description;
            partner.Website = website;
            partner.LogoPath = logoPath;
            partner.AddedOn = DateTime.Now;
            partner.IsActive = true;

            _db.Partners.Add(partner);
            _db.SaveChanges();
            TempData["Success"] = "Partner added.";
            return RedirectToAction("ManagePartners");
        }
        public IActionResult EditPartner(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Partner? partner = _db.Partners.FirstOrDefault(p => p.PartnerId == id);
            if (partner == null) return RedirectToAction("ManagePartners");
            return View(partner);
        }
        [HttpPost]
        public IActionResult EditPartner(int id, string companyName, string description, string website, bool isActive, IFormFile logoFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Partner? partner = _db.Partners.FirstOrDefault(p => p.PartnerId == id);
            if (partner == null) return RedirectToAction("ManagePartners");
            if (logoFile != null && logoFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    logoFile.CopyTo(stream);
                }
                partner.LogoPath = "/images/" + fileName;
            }
            partner.CompanyName = companyName;
            partner.Description = description;
            partner.Website = website;
            partner.IsActive = isActive;

            _db.SaveChanges();
            TempData["Success"] = "Partner updated.";
            return RedirectToAction("ManagePartners");
        }
        public IActionResult DeletePartner(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Partner? partner = _db.Partners.FirstOrDefault(p => p.PartnerId == id);
            if (partner != null)
            {
                _db.Partners.Remove(partner);
                _db.SaveChanges();
                TempData["Success"] = "Partner deleted.";
            }
            return RedirectToAction("ManagePartners");
        }
        public IActionResult ManageEvents()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var events = _db.Events.OrderByDescending(e => e.EventDate).ToList();
            return View(events);
        }
        public IActionResult AddEvent()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            return View();
        }
        [HttpPost]
        public IActionResult AddEvent(string title, string description, string category, string location, DateTime eventDate, bool isUpcoming, IFormFile imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            if (string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error = "Event title is required.";
                return View();
            }
            string imagePath = "/images/default-event.jpg";
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                imagePath = "/images/"+fileName;
            }
            Event newEvent = new Event();
            newEvent.Title = title;
            newEvent.Description = description;
            newEvent.Category = category;
            newEvent.Location = location;
            newEvent.EventDate = eventDate;
            newEvent.IsUpcoming = isUpcoming;
            newEvent.ImagePath = imagePath;
            newEvent.CreatedAt = DateTime.Now;

            _db.Events.Add(newEvent);
            _db.SaveChanges();
            TempData["Success"] = "Event added.";
            return RedirectToAction("ManageEvents");
        }
        public IActionResult EditEvent(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Event? ev = _db.Events.FirstOrDefault(e => e.EventId == id);
            if (ev == null)
            {
                return RedirectToAction("ManageEvents");
            }
            return View(ev);
        }
        [HttpPost]
        public IActionResult EditEvent(int id, string title, string description,string category, string location,DateTime eventDate, bool isUpcoming,IFormFile imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Event? ev = _db.Events.FirstOrDefault(e => e.EventId == id);
            if (ev == null) return RedirectToAction("ManageEvents");
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                ev.ImagePath = "/images/" + fileName;
            }
            ev.Title = title;
            ev.Description = description;
            ev.Category = category;
            ev.Location = location;
            ev.EventDate = eventDate;
            ev.IsUpcoming = isUpcoming;

            _db.SaveChanges();
            TempData["Success"] = "Event updated.";
            return RedirectToAction("ManageEvents");
        }
        public IActionResult DeleteEvent(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Event? ev = _db.Events.FirstOrDefault(e => e.EventId == id);
            if (ev != null)
            {
                _db.Events.Remove(ev);
                _db.SaveChanges();
                TempData["Success"] = "Event deleted.";
            }
            return RedirectToAction("ManageEvents");
        }
        public IActionResult ManageQueries()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var queries = _db.Queries.OrderBy(q => q.IsRepliedAt).ThenByDescending(q => q.SubmittedAt).ToList();
            return View(queries);
        }
        public IActionResult ReplyQuery(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Query? query = _db.Queries.FirstOrDefault(q => q.QueryId == id);
            if (query == null) return RedirectToAction("ManageQueries");
            return View(query);
        }
        [HttpPost]
        public IActionResult ReplyQuery(int id, string adminReply)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            Query? query = _db.Queries.FirstOrDefault(q => q.QueryId == id);
            if (query == null) return RedirectToAction("ManageQueries");
            if (string.IsNullOrWhiteSpace(adminReply))
            {
                ViewBag.Error = "Reply cannot be empty!";
                return View(query);
            }
            query.AdminReply = adminReply;
            query.IsRepliedAt = true;
            query.RepliedAt = DateTime.Now;

            _db.SaveChanges();
            TempData["Success"] = "Reply sent!";
            return RedirectToAction("ManageQueries");
        }
        public IActionResult ManageMessages()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var messages = _db.ContactMessages.OrderBy(c => c.IsRead).ThenByDescending(c => c.SubmittedAt).ToList();
            return View(messages);
        }
        public IActionResult MarkAsRead(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            ContactMessage? message = _db.ContactMessages.FirstOrDefault(c => c.ContactMessageId == id);
            if (message != null)
            {
                message.IsRead = true;
                _db.SaveChanges();
            }
            return RedirectToAction("ManageMessages");
        }
        public IActionResult DeleteMessage(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            ContactMessage? message = _db.ContactMessages.FirstOrDefault(c => c.ContactMessageId == id);
            if (message != null)
            {
                _db.ContactMessages.Remove(message);
                _db.SaveChanges();
                TempData["Success"] = "Message deleted.";
            }
            return RedirectToAction("ManageMessages");
        }
        public IActionResult ManageDonations()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var donations = _db.Donations.OrderByDescending(d => d.DonationDate).ToList();
            ViewBag.TotalAmount = _db.Donations.Sum(d => d.Amount);
            return View(donations);
        }
    }
}