namespace BuzzTalk.Server.Models
{
    public class UserModelHub
    {
            
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string ConnectionId { get; set; } = null!;
    }
}
