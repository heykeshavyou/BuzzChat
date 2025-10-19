using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Entities;
using BuzzTalk.Data.Repositries;

namespace BuzzTalk.Business.Services
{
    public interface IMessageService
    {
       Task<(bool, string, MessageDto)> SendMessage(MessageDto message);
       Task<List<MessageDto>> GetAllMessages(int fromId,int toId);
       Task<List<MessageDto>> MarkRead(int fromId, int toId);
    }
    public class MessageService : IMessageService
    {
        private readonly IMessageRepositry _message;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepositry message,IMapper mapper)
        {
            _message = message;
            _mapper = mapper;
        }

        public async Task<List<MessageDto>> GetAllMessages(int fromId, int toId)
        {
            var messages = await _message.GetAllMessages(fromId, toId);
            if (messages != null)
            {
                return _mapper.Map<List<MessageDto>>(messages);
            }
            return null;
        }

        public async Task<List<MessageDto>> MarkRead(int fromId, int toId)
        {
            var messages = await _message.MarkRead(fromId, toId);
            if (messages != null)
            {
                return _mapper.Map<List<MessageDto>>(messages);
            }
            return null;
        }

        public async Task<(bool, string, MessageDto)> SendMessage(MessageDto message)
        {
            var messageEntity = _mapper.Map<Message>(message);
            var result = await _message.SendMessage(messageEntity);
            if (result.Item1)
            {
                return (true, result.Item2, _mapper.Map<MessageDto>(result.Item3));
            }
            return (false, result.Item2, null);
        }
    }
}
