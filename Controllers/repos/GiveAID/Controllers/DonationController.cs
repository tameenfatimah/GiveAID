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
            //User must be logged in to make a donation
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ReturnMessage"]= "Please log-in to make a donation.";
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Index(decimal amount, string cause,
                                   string cardHolder, string cardNumber,
                                   string cardExpiry, string cardCvv,
                                   string cardType)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            // Basic validation for donation amount
            if (amount <= 0 )
            {
                ViewBag.Error = "Please enter a valid donation amount.";
                return View();
            }
            // Basic validation for cause selection
            if (string.IsNullOrWhiteSpace(cause))
            {
                ViewBag.Error = "Please select a cause for your donation.";
                return View();
            }
            // Basic validation for card details
            if (string.IsNullOrWhiteSpace(cardHolder))
            {
                ViewBag.Error = "Please enter the card holder name.";
                return View();
            }
            //Card number validation (basic check for length and digits)
            //Removes spaces and dashes from the card number for validation
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");
            if (string.IsNullOrWhiteSpace(cardNumber)||cardNumber.Length!=16)
            {
                ViewBag.Error = "Please enter a valid 16-digit card number.";
                return View();
            }
            //Make sure card number contains only digits
            if (!cardNumber.All(char.IsDigit))
            {
                ViewBag.Error = "Card number must contain digits only.";
                return View();
            }
            // Basic validation for card expiry (MM/YY format)
            if (string.IsNullOrWhiteSpace(cardExpiry))
            {
                ViewBag.Error = "Please enter the card expiry date.";
                return View();
            }
            // Basic validation for card CVV (3 or 4 digits)
            if (string.IsNullOrWhiteSpace(cardCvv))
            {
                ViewBag.Error = "Please enter the card CVV.";
                return View();
            }
            if (!cardCvv.All(char.IsDigit))
            {
                ViewBag.Error = "CVV must contain digits only";
                return View();
            }
            // Basic validation for card type
            if(string.IsNullOrWhiteSpace(cardType))
            {
                ViewBag.Error = "Please select a card type.";
                return View();
            }
            // If all validations pass, create a new donation record
            var donation = new Donation
            {
                UserId = userId.Value,
                Amount = amount,
                Cause = cause,
                CardHolder = cardHolder,
                CardNumber = cardNumber,   // dummy - not real payment
                CardType = cardType,
                DonationDate = DateTime.Now
            };
            _db.Donations.Add(donation);
            _db.SaveChanges();

            // Pass donation details to Success page via TempData
            TempData["DonationAmount"] = amount.ToString();
            TempData["DonationCause"] = cause;
            return RedirectToAction("Success");
        }
        public IActionResult Success()
        {
            /* If someone visits Donation/Success directly
             without donating, send them to donation form*/
            if (TempData["DonationAmount"] == null)
            {
                return RedirectToAction("Index");
            }

            // Pass the amount and cause to the view
            ViewBag.Amount = TempData["DonationAmount"];
            ViewBag.Cause = TempData["DonationCause"];

            return View();
        }
    }
}
