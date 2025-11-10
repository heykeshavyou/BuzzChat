namespace BuzzTalk.Server.Models
{
    public class GroupUserModel
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public bool? Isjoined { get; set; }
    }
}
