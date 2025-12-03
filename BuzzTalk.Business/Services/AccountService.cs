using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Entities;
using BuzzTalk.Data.Repositories;

namespace BuzzTalk.Business.Services
{
    public interface IAccountService
    {
         Task<(bool, string,int)> SignIn(UserDto  user);
        Task<UserDto> Login(string username,string password, string token);
        Task<List<UserDto>> GetAllUsers(int id);
        Task<(bool, string)> SaveToken(string token,int id);
    }
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepositry;
        private readonly IMapper _mapper;
        public AccountService(IAccountRepository accountRepositry, IMapper mapper)
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

        public async Task<UserDto> Login(string username, string password, string token)
        {
            var user = await _accountRepositry.Login(username, password,token);
            if (user != null)
            {
                return _mapper.Map<UserDto>(user);
            }
            return null;
        }

        public async Task<(bool, string)> SaveToken(string token, int id)
        {
            var res = await _accountRepositry.SaveToken(token, id);
            return res;
        }

        public async Task<(bool, string,int)> SignIn(UserDto user)
        {
            var userModel = _mapper.Map<User>(user);
            return await _accountRepositry.SignIn(userModel);
        }
    }
}
