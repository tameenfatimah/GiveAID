using System.ComponentModel.DataAnnotations.Schema;

namespace GiveAID.Models
{
    public class Donation
    {
        public int DonationId { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string Cause { get; set; }   // e.g., "Education", "Children", "Disabled"
        public DateTime DonationDate { get; set; }
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public User user { get; set; }  /*Not a actual table column, but a navigation property
                                        to access the User details associated with the donation*/
    }
}
