using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Entities;

namespace BuzzTalk.Business
{
    public class MapperDto : AutoMapper.Profile
    {
        public MapperDto()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();
            CreateMap<Message,MessageDto>()
                .ReverseMap();
        }
    }
}
