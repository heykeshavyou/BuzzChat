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
            CreateMap<Message, MessageDto>()
                .ReverseMap();
            CreateMap<GroupDto, Group>()
                .ForMember(dest => dest.GroupUsers,
                opt => opt.MapFrom(src => src.Users.Select(u => new GroupUser
                {
                    UserId = u.Id
         }).ToList()))
     .ReverseMap()
     .ForMember(dest => dest.Users,
         opt => opt.MapFrom(src => src.GroupUsers.Select(gu => gu.User).ToList()));
        }
    }
}
