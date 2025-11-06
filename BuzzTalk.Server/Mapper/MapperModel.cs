using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Server.Models;

namespace BuzzTalk.Server.Mapper
{
    public class MapperModel:Profile
    {
        public MapperModel()
        {
            CreateMap<UserModel, UserDto>()
                .ReverseMap();
            CreateMap<MessageModel, MessageDto>()
                .ReverseMap();
            CreateMap<SigninModel, UserDto>();
            CreateMap<UserModel, UserModelHub>();
            CreateMap<MessageHub,MessageDto>().ReverseMap();
            CreateMap<GroupModel, GroupDto>().ReverseMap();
        }
    }
}
