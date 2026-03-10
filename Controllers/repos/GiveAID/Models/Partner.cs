namespace GiveAID.Models
{
    public class Partner
    {
        public int PartnerId { get; set; }
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public string LogoPath { get; set; }
        public string Description { get; set; }
        public DateTime AddedOn { get; set; }   //Time when Admin added the Partner
        public bool IsActive { get; set; }  // Indicates if the Partner is currently active and collaborating with GiveAID
    }
}
