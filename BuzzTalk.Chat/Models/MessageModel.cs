namespace BuzzTalk.Chat.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime SentOn { get; set; }
        public int FromId { get; set; }
        public bool IsRead { get; set; }

        public int ToId { get; set; }
    }
}
