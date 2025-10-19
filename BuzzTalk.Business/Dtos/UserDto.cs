namespace BuzzTalk.Business.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Username { get; set; } = null!;

        public DateTime? JoinedOn { get; set; }

        public virtual ICollection<MessageDto> MessageFroms { get; set; } = new List<MessageDto>();

        public virtual ICollection<MessageDto> MessageTos { get; set; } = new List<MessageDto>();
    }
}
