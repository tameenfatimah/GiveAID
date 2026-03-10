namespace GiveAID.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public string Role { get; set; }    //Admin or User
        public DateTime RegisteredAt { get; set; }
    }
}
