namespace BuzzTalk.Server.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime SentOn { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public bool IsRead { get; set; }

        public virtual UserModel FromUser { get; set; } = null!;
        public virtual UserModel ToUser { get; set; } = null!;
    }
}
