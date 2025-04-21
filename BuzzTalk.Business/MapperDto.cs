using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Models;

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
