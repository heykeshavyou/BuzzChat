using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Entities;

namespace BuzzTalk.Server.Models
{
    public class GroupModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string Icon { get; set; } = null!;

        public int? CreatedBy { get; set; }

        public string Guid { get; set; } = null!;
        public List<UserModel> Users { get; set; } = new List<UserModel>();
        public virtual ICollection<MessageHub> Messages { get; set; } = new List<MessageHub>();

    }
}
