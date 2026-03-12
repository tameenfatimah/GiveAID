namespace GiveAID.Models
{
    public class ContactMessage
    {
        public int ContactMessageId { get; set; }   
        public string FullName { get; set; }        
        public string guestEmail { get; set; }            
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
