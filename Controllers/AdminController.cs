using GiveAID.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }
        //Helper method to check if the user is an admin
        //Required to be called at the very beginning of each action method to ensure security
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }
        public IActionResult Dashboard()
        {
            if(!IsAdmin()) return RedirectToAction("Login", "User");
            // Fetching some statistics for the dashboard
            ViewBag.TotalUsers = _db.Users.Count();
            ViewBag.TotalDonations = _db.Donations.Count();
            ViewBag.TotalAmount = _db.Donations.Sum(d => d.Amount);
            ViewBag.TotalNGOs = _db.NGOs.Count();
            ViewBag.TotalEvents = _db.Events.Count();
            ViewBag.TotalPartners = _db.Partners.Count();
            ViewBag.TotalGallery = _db.Galleries.Count();
            ViewBag.PendingQueries = _db.Queries.Count(q => q.IsRepliedAt == false);
            ViewBag.UnreadMessages = _db.ContactMessages.Count(c => c.IsRead == false);

            //Recent 5 donation to display on dashboard
            ViewBag.RecentDonations = _db.Donations
                .OrderByDescending(d => d.DonationDate)
                .Take(5)
                .ToList();  
            return View();
        }
        public IActionResult ManageUsers()
        {
            if(!IsAdmin()) return RedirectToAction("Login", "User");
            var users = _db.Users
                .OrderByDescending(u => u.RegisteredAt)
                .ToList();
            return View();
        }
        public IActionResult DeleteUser(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var user = _db.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("ManageUsers");
            }
            // Prevent admin from deleting themselves
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (user.UserId == currentUserId)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction("ManageUsers");
            }
            _db.Users.Remove(user);
            _db.SaveChanges();

            TempData["Success"] = "User deleted successfully.";
            return RedirectToAction("ManageUsers");
        }
        public IActionResult ManageNGOs()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            var ngos = _db.NGOs
                .OrderBy(n => n.Name)
                .ToList();
            return View(ngos);
        }
        public IActionResult AddNGO()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            return View();
        }
        [HttpPost]
        public IActionResult AddNGO(string name, string description,
                                     string contactEmail, string contactPhone,
                                     string website, IFormFile logoFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "NGO name is required.";
                return View();
            }
            // Handle logo upload
            string logoPath = "/images/default-ngo.png";
            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    logoFile.CopyTo(stream);
                }
                logoPath = "/images/" + fileName;
            }
            var ngo = new NGO
            {
                Name = name,
                Description = description,
                ContactEmail = contactEmail,
                ContactPhone = contactPhone,
                Website = website,
                LogoPath = logoPath,
                AddedOn = DateTime.Now,
                IsActive = true
            };
            _db.NGOs.Add(ngo);
            _db.SaveChanges();

            TempData["Success"] = "NGO added successfully.";
            return RedirectToAction("ManageNGOs");
        }
        public IActionResult EditNGO(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var ngo = _db.NGOs.FirstOrDefault(n => n.NGOId == id);
            if (ngo == null) return RedirectToAction("ManageNGOs");

            return View(ngo);
        }
        [HttpPost]
        public IActionResult EditNGO(int id, string name, string description,
                                      string contactEmail, string contactPhone,
                                      string website, bool isActive,
                                      IFormFile logoFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var ngo = _db.NGOs.FirstOrDefault(n => n.NGOId == id);
            if (ngo == null) return RedirectToAction("ManageNGOs");

            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "NGO name is required.";
                return View(ngo);
            }

            // Update logo only if a new file was uploaded
            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
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

            TempData["Success"] = "NGO updated successfully!";
            return RedirectToAction("ManageNGOs");
        }
        public IActionResult DeleteNGO(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var ngo = _db.NGOs.FirstOrDefault(n => n.NGOId == id);
            if (ngo != null)
            {
                _db.NGOs.Remove(ngo);
                _db.SaveChanges();
                TempData["Success"] = "NGO deleted successfully.";
            }
            return RedirectToAction("ManageNGOs");
        }
        public IActionResult ManageGallery()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var images = _db.Galleries
                            .OrderByDescending(g => g.UploadedAt)
                            .ToList();
            return View(images);
        }
        public IActionResult UploadImage()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            return View();
        }
        [HttpPost]
        public IActionResult UploadImage(string caption, string category,
                                          int? eventId, IFormFile imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            if (imageFile == null || imageFile.Length == 0)
            {
                ViewBag.Error = "Please select an image to upload.";
                return View();
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            var gallery = new Gallery
            {
                ImagePath = "/images/" + fileName,
                Caption = caption,
                Category = category,
                EventId = eventId,
                UploadedAt = DateTime.Now
            };

            _db.Galleries.Add(gallery);
            _db.SaveChanges();

            TempData["Success"] = "Image uploaded successfully!";
            return RedirectToAction("ManageGallery");
        }
        public IActionResult DeleteImage(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var image = _db.Galleries.FirstOrDefault(g => g.GalleryId == id);
            if (image != null)
            {
                // Delete physical file from wwwroot too
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                            "wwwroot" + image.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _db.Galleries.Remove(image);
                _db.SaveChanges();
                TempData["Success"] = "Image deleted successfully.";
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
        public IActionResult AddPartner(string companyName, string description,
                                        string website, IFormFile logoFile)
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
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    logoFile.CopyTo(stream);
                }
                logoPath = "/images/" + fileName;
            }

            var partner = new Partner
            {
                CompanyName = companyName,
                Description = description,
                Website = website,
                LogoPath = logoPath,
                AddedOn = DateTime.Now,
                IsActive = true
            };

            _db.Partners.Add(partner);
            _db.SaveChanges();

            TempData["Success"] = "Partner added successfully!";
            return RedirectToAction("ManagePartners");
        }
        public IActionResult EditPartner(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var partner = _db.Partners.FirstOrDefault(p => p.PartnerId == id);
            if (partner == null) return RedirectToAction("ManagePartners");

            return View(partner);
        }
        [HttpPost]
        public IActionResult EditPartner(int id, string companyName,
                                                  string description, string website,
                                                  bool isActive, IFormFile logoFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var partner = _db.Partners.FirstOrDefault(p => p.PartnerId == id);
            if (partner == null) return RedirectToAction("ManagePartners");

            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
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

            TempData["Success"] = "Partner updated successfully!";
            return RedirectToAction("ManagePartners");
        }
        public IActionResult DeletePartner(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var partner = _db.Partners.FirstOrDefault(p => p.PartnerId == id);
            if (partner != null)
            {
                _db.Partners.Remove(partner);
                _db.SaveChanges();
                TempData["Success"] = "Partner deleted successfully.";
            }
            return RedirectToAction("ManagePartners");
        }
        public IActionResult ManageEvents()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var events = _db.Events
                            .OrderByDescending(e => e.EventDate)
                            .ToList();
            return View(events);
        }
        public IActionResult AddEvent()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");
            return View();
        }
        [HttpPost]
        public IActionResult AddEvent(string title, string description,
                                       string category, string location,
                                       DateTime eventDate, bool isUpcoming,
                                       IFormFile imageFile)
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
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                imagePath = "/images/" + fileName;
            }

            var newEvent = new Event
            {
                Title = title,
                Description = description,
                Category = category,
                Location = location,
                EventDate = eventDate,
                IsUpcoming = isUpcoming,
                ImagePath = imagePath,
                CreatedAt = DateTime.Now
            };

            _db.Events.Add(newEvent);
            _db.SaveChanges();

            TempData["Success"] = "Event added successfully!";
            return RedirectToAction("ManageEvents");
        }
        public IActionResult EditEvent(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var ev = _db.Events.FirstOrDefault(e => e.EventId == id);
            if (ev == null) return RedirectToAction("ManageEvents");

            return View(ev);
        }
        [HttpPost]
        public IActionResult EditEvent(int id, string title, string description,
                                        string category, string location,
                                        DateTime eventDate, bool isUpcoming,
                                        IFormFile imageFile)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var ev = _db.Events.FirstOrDefault(e => e.EventId == id);
            if (ev == null) return RedirectToAction("ManageEvents");

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
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

            TempData["Success"] = "Event updated successfully!";
            return RedirectToAction("ManageEvents");
        }
        public IActionResult DeleteEvent(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var ev = _db.Events.FirstOrDefault(e => e.EventId == id);
            if (ev != null)
            {
                _db.Events.Remove(ev);
                _db.SaveChanges();
                TempData["Success"] = "Event deleted successfully.";
            }
            return RedirectToAction("ManageEvents");
        }
        public IActionResult ManageQueries()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var queries = _db.Queries
                             .OrderBy(q => q.IsRepliedAt)        // pending ones first
                             .ThenByDescending(q => q.SubmittedAt)
                             .ToList();
            return View(queries);
        }
        public IActionResult ReplyQuery(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var query = _db.Queries.FirstOrDefault(q => q.QueryId == id);
            if (query == null) return RedirectToAction("ManageQueries");

            return View(query);
        }
        [HttpPost]
        public IActionResult ReplyQuery(int id, string adminReply)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var query = _db.Queries.FirstOrDefault(q => q.QueryId == id);
            if (query == null) return RedirectToAction("ManageQueries");

            if (string.IsNullOrWhiteSpace(adminReply))
            {
                ViewBag.Error = "Reply cannot be empty.";
                return View(query);
            }

            query.AdminReply = adminReply;
            query.IsRepliedAt = true;
            query.RepliedAt = DateTime.Now;

            _db.SaveChanges();

            TempData["Success"] = "Reply sent successfully!";
            return RedirectToAction("ManageQueries");
        }
        public IActionResult ManageMessages()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var messages = _db.ContactMessages
                              .OrderBy(c => c.IsRead)          // unread ones first
                              .ThenByDescending(c => c.SubmittedAt)
                              .ToList();
            return View(messages);
        }
        public IActionResult MarkAsRead(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "User");

            var message = _db.ContactMessages.FirstOrDefault(c => c.ContactMessageId == id);
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

            var message = _db.ContactMessages.FirstOrDefault(c => c.ContactMessageId == id);
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

            var donations = _db.Donations
                               .OrderByDescending(d => d.DonationDate)
                               .ToList();

            // Pass total amount to view
            ViewBag.TotalAmount = _db.Donations.Sum(d => d.Amount);

            return View(donations);
        }
    }
}
