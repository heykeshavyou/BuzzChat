using BuzzTalk.Data.Entities;
using BuzzTalk.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BuzzTalk.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<(bool, string, int)> SignIn(User user);
        Task<User> Login(string username, string password, string token);
        Task<List<User>> GetAllUsers(int id);
        Task<(bool, string)> SaveToken(string token, int id);
        Task<(bool,string)> ChangeName(string name, int id);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly BuzzTalkContext _db;
        public AccountRepository(BuzzTalkContext buzz)
        {
            _db = buzz;

        }

        public async Task<List<User>> GetAllUsers(int id)
        {
            var GetAllUsers = await _db.Users.Where(x => x.Id != id).ToListAsync();
            if (GetAllUsers != null)
            {
                return GetAllUsers;
            }
            return null;
        }

        public async Task<User> Login(string username, string password, string token)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == username.ToLower() && x.Password == PasswordHelper.EncryptPassword(password));
                if (user != null)
                {
                    user.Fcm = token;
                    await _db.SaveChangesAsync();
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<(bool, string, int)> SignIn(User user)
        {
            var GetUserName = await _db.Users.FirstOrDefaultAsync(x => x.Username == user.Username.ToLower());
            if (GetUserName != null)
            {
                return (false, "UserName already exists", 0);
            }
            var GetEmail = await _db.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (GetEmail != null)
            {
                return (false, "Email already exists", 0);
            }
            user.Username = user.Username.ToLower();
            user.Email = user.Email.ToLower();
            user.JoinedOn = DateTime.UtcNow;
            user.Password = PasswordHelper.EncryptPassword(user.Password);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return (true, "User Created Successfully", user.Id);
        }
        public async Task<(bool, string)> SaveToken(string token, int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                user.Fcm = token;
                await _db.SaveChangesAsync();
                return (true, "Token Saved");
            }
            return (false, "Not found");

        }

        public async Task<(bool, string)> ChangeName(string name, int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x=>x.Id== id);
            if (user != null)
            {
                user.Name = name;
                await _db.SaveChangesAsync();
                return (true, "Name Changed");
            }
            return (false, "User not found");

        }
    }
}
