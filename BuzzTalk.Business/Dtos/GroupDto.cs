using BuzzTalk.Data.Entities;

namespace BuzzTalk.Business.Dtos
{
    public class GroupDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string Icon { get; set; } = null!;

        public int? CreatedBy { get; set; }

        public string Guid { get; set; } = null!;
        public List<UserDto> Users { get; set; }=new List<UserDto>();
        public virtual ICollection<MessageDto> Messages { get; set; } = new List<MessageDto>();

    }
}
