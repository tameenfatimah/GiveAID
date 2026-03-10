namespace GiveAID.Models
{
    public class NGO
    {
        public int NGOId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public string ContactPhone { get; set; }
        public DateTime AddedOn { get; set; }   //Time when Admin added the NGO
        public bool IsActive { get; set; }  // Indicates if the NGO is currently active and accepting donations
    }
}
