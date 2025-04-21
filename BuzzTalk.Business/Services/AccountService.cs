using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Models;
using BuzzTalk.Data.Repositries;

namespace BuzzTalk.Business.Services
{
    public interface IAccountService
    {
         Task<(bool, string,int)> SignIn(UserDto  user);
        Task<UserDto> Login(string username,string password);
        Task<List<UserDto>> GetAllUsers(int id);
    }
    public class AccountService : IAccountService
    {
        private readonly IAccountRepositry _accountRepositry;
        private readonly IMapper _mapper;
        public AccountService(IAccountRepositry accountRepositry, IMapper mapper)
        {
            _accountRepositry = accountRepositry;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsers(int id)
        {
            var users = await _accountRepositry.GetAllUsers(id);
            if (users != null)
            {
                return _mapper.Map<List<UserDto>>(users);
            }
            return null;
        }

        public async Task<UserDto> Login(string username, string password)
        {
            var user = await _accountRepositry.Login(username, password);
            if (user != null)
            {
                return _mapper.Map<UserDto>(user);
            }
            return null;
        }

        public async Task<(bool, string,int)> SignIn(UserDto user)
        {
            var userModel = _mapper.Map<User>(user);
            return await _accountRepositry.SignIn(userModel);
        }
    }
}
