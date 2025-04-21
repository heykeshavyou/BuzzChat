using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuzzTalk.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BuzzTalk.Data.Repositries
{
    public interface IMessageRepositry
    {
       Task<(bool, string,Message)> SendMessage(Message message);
       Task<List<Message>> GetAllMessages(int fromId, int toId);
        Task<List<Message>> MarkRead(int fromId, int toId);
    }
    public class MessageRepositry : IMessageRepositry
    {
        private readonly BuzzTalkContext _context;

        public MessageRepositry(BuzzTalkContext context)
        {
            _context = context;
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
