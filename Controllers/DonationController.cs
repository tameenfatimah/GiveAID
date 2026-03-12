using GiveAID.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DonationController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ReturnMessage"] = "You need to login before making a donation.";
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Index(decimal amount, string cause, string cardHolder, string cardNumber, string cardExpiry, string cardCvv, string cardType)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (amount <= 0)
            {
                ViewBag.Error = "Please enter a valid amount.";
                return View();
            }
            if (string.IsNullOrWhiteSpace(cause))
            {
                ViewBag.Error = "Please select a cause.";
                return View();
            }
            if (string.IsNullOrWhiteSpace(cardHolder))
            {
                ViewBag.Error = "Please enter card holder name , it is required.";
                return View();
            }
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length != 16)
            {
                ViewBag.Error = "Card Number must be of 16 digits.";
                return View();
            }
            if (string.IsNullOrWhiteSpace(cardExpiry))
            {
                ViewBag.Error = "Please enter expiry date.";
                return View();
            }
            if (string.IsNullOrWhiteSpace(cardCvv))
            {
                ViewBag.Error = "Please enter CVV";
                return View();
            }
            if (string.IsNullOrWhiteSpace(cardType))
            {
                ViewBag.Error = "Please select card type.";
                return View();
            }
            Donation newDonation = new Donation();
            newDonation.UserId = userId.Value;
            newDonation.Amount = amount;
            newDonation.Cause = cause;
            newDonation.CardHolder = cardHolder;
            newDonation.CardNumber = cardNumber;
            newDonation.CardType = cardType;
            newDonation.DonationDate = DateTime.Now;

            _db.Donations.Add(newDonation);
            _db.SaveChanges();

            TempData["DonationAmount"] = Convert.ToString(amount);
            TempData["DonationCause"] = cause;

            return RedirectToAction("Success");
        }
        public IActionResult Success()
        {
            if(TempData["DonationAmount"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Amount = TempData["DonationAmount"];
            ViewBag.Cause = TempData["DonationCause"];
            return View();
        }
    }
}