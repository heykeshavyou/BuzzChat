using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuzzTalk.Data.Helpers;
using BuzzTalk.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BuzzTalk.Data.Repositries
{
    public interface IAccountRepositry
    {
        Task<(bool, string, int)> SignIn(User user);
        Task<User> Login(string username ,string password);
        Task<List<User>> GetAllUsers(int id);
    }
    public class AccountRepositry : IAccountRepositry
    {
        private readonly BuzzTalkContext _buzz;
        public AccountRepositry(BuzzTalkContext buzz)
        {
            _buzz = buzz;

        }

        public async Task<List<User>> GetAllUsers(int id)
        {
            var GetAllUsers = await _buzz.Users.Where(x=>x.Id!=id).ToListAsync();
            if (GetAllUsers != null)
            {
                return GetAllUsers;
            }
            return null;
        }

        public Task<User> Login(string username ,string password )
        {
            var GetUser = _buzz.Users.FirstOrDefaultAsync(x => x.Username == username.ToLower() && x.Password == PasswordHelper.EncryptPassword(password));
            if (GetUser != null)
            {
                return GetUser;
            }
            return null;
        }

        public async Task<(bool, string,int)> SignIn(User user)
        {
           var GetUserName = await _buzz.Users.FirstOrDefaultAsync(x => x.Username == user.Username.ToLower() );
            if (GetUserName != null)
            {
                return (false, "UserName already exists", 0);
            }
            var GetEmail = await _buzz.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (GetEmail != null)
            {
                return (false, "Email already exists",0);
            }
            user.Username = user.Username.ToLower();
            user.Email = user.Email.ToLower();
            user.JoinedOn = DateTime.UtcNow;
            user.Password = PasswordHelper.EncryptPassword(user.Password);
            await _buzz.Users.AddAsync(user);
            await _buzz.SaveChangesAsync();
            return (true, "User Created Successfully",user.Id);
        }
    }
}
