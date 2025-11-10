
namespace BuzzTalk.Server.Models
{
    public class CreateGroupModel
    {
        public string Name { get; set; }
        public IFormFile icon { get; set; }
        public List<int> users { get; set; }
    }
}
