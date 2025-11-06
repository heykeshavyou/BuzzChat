using BuzzTalk.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuzzTalk.Data.Repositories
{
    public interface IMessageRepository
    {
       Task<(bool, string,Message)> SendMessage(Message message);
       Task<List<Message>> GetAllMessages(int fromId, int toId);
        Task<List<Message>> MarkRead(int fromId, int toId);
    }
    public class MessageRepository : IMessageRepository
    {
        private readonly BuzzTalkContext _context;
        private readonly IGroupRepository _groupRepository;

        public MessageRepository(BuzzTalkContext context, IGroupRepository groupRepository)
        {
            _context = context;
            _groupRepository = groupRepository;
        }

        public async Task<List<Message>> GetAllMessages(int fromId, int toId)
        {
            var messages = await _context.Messages
                .Where(x=>(x.FromId== fromId && x.ToId==toId)|| (x.FromId == toId && x.ToId == fromId))
                .OrderBy(x => x.SentOn)
                .ToListAsync();
            return messages;
        }

        public async Task<List<Message>> MarkRead(int fromId, int toId)
        {
            var messages = await _context.Messages
                .Where(x => x.FromId == toId && x.ToId == fromId && x.IsRead == false)
                .ToListAsync();
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    message.IsRead = true;
                }
                await _context.SaveChangesAsync();
            }
            return messages;
        }

        public async Task<(bool, string, Message)> SendMessage(Message message)
        {
            try
            {
                if (message.GroupId == null)
                {
                   var res =await _groupRepository.CreateOneToOneChat((int)message.ToId, (int)message.FromId);
                    if(res != null)
                    {
                        message.GroupId=res.Id;
                    }
                    else
                    {
                        return (false, "Message not sent", message);
                    }
                }
                message.SentOn = DateTime.Now;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return (true, "Message sent successfully", message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message,null);
            }
        }
    }
}
