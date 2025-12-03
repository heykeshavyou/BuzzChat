using BuzzTalk.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace BuzzTalk.Data.Repositories
{
    public interface IGroupRepository
    {
        Task<Group> CreateGroup(Group group, List<int> users);
        Task<Group> CreateOneToOneChat(int toId, int fromId);
        Task<List<Group>> GetAllGroups(int userId);
        Task<Group> GetGroupById(int id);
        Task<List<(int Id, string Token)>> GetGroupAllUserToken(int id, int userId);


    }


    public class GroupRepository : IGroupRepository
    {
        private readonly BuzzTalkContext _db;
        public GroupRepository(BuzzTalkContext db)
        {
            _db = db;
        }
        public async Task<Group> CreateGroup(Group group, List<int> users)
        {
            group.Guid = GuidGenerator();
            group.CreatedTime = DateTime.Now;
            await _db.Groups.AddAsync(group);
            await _db.SaveChangesAsync();
            var groupusers = new List<GroupUser>();
            foreach (var a in users)
            {
                groupusers.Add(new GroupUser()
                {
                    GroupId = group.Id,
                    UserId = a,
                    Isjoined = true,
                });
            }
            await _db.GroupUsers.AddRangeAsync(groupusers);
            await _db.SaveChangesAsync();
            return await GetGroupById(group.Id);
        }

        public async Task<Group> CreateOneToOneChat(int toId, int fromId)
        {
            var user1 = await _db.Users.FirstOrDefaultAsync(x => x.Id == fromId);
            if (user1 is null) return null;
            var user2 = await _db.Users.FirstOrDefaultAsync(x => x.Id == toId);
            if (user2 is null) return null;
            Group group = new Group()
            {
                Guid = GuidGenerator(),
                Name = null,
                CreatedBy = null,
                CreatedTime = DateTime.Now,
                Icon = ""
            };

            await _db.Groups.AddAsync(group);
            await _db.SaveChangesAsync();
            var newGroupUser = new List<GroupUser>()
            {
                new GroupUser
                {
                    UserId = user2.Id,
                    GroupId = group.Id,
                    Isjoined=true
                },
                new GroupUser
                {
                    UserId = user1.Id,
                    GroupId = group.Id,
                    Isjoined=true
                },
            };
            await _db.GroupUsers.AddRangeAsync(newGroupUser);
            await _db.SaveChangesAsync();
            group.GroupUsers = newGroupUser;
            return group;
        }

        public async Task<List<Group>> GetAllGroups(int userId)
        {
            var groups = await _db.GroupUsers
                .Include(x => x.Group)
                .ThenInclude(x => x.GroupUsers)
                .ThenInclude(x => x.User)
                .Include(x => x.Group.Messages)
                .Where(x => x.UserId == userId && x.Isjoined == true)
                .Select(x => x.Group)
                .OrderByDescending(g => g.Messages
                .OrderByDescending(m => m.SentOn)
                .Select(m => m.SentOn)
                .FirstOrDefault())
                .ToListAsync();
            return groups;
        }

        public async Task<List<(int Id,string Token)>> GetGroupAllUserToken(int id, int userId)
        {
            var tokens = await _db.GroupUsers
                .Include(x => x.User)
                .Where(x => x.GroupId == id && x.UserId != userId)
                .Select(x => new ValueTuple<int,string>
                (
                     x.User.Id,
                    x.User.Fcm
                ))
                .ToListAsync();
            return tokens;
        }

        public async Task<Group> GetGroupById(int id)
        {
            var group = await _db.Groups
                .Include(x => x.GroupUsers)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            return group;
        }

        private string GuidGenerator()
        {
            Guid newGuid = Guid.NewGuid();
            return $"{newGuid}";
        }
    }
}
