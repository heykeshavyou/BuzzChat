namespace BuzzTalk.Server.Models
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
        public virtual ICollection<MessageModel> MessageFroms { get; set; } = new List<MessageModel>();
        public virtual ICollection<MessageModel> MessageTos { get; set; } = new List<MessageModel>();
    }
}
