namespace BuzzTalk.Chat.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;
        public DateTime? JoinedOn { get; set; }
        public string? Token { get; set; }
    }
}
