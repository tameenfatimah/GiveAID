namespace GiveAID.Models
{
    public class Query
    {
        public int QueryId { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string? AdminReply { get; set; }
        public bool IsRepliedAt { get; set; }
        public DateTime SubmittedAt { get; set; }   
        public DateTime? RepliedAt { get; set; } 
        public User user { get; set; }  
    }
}
