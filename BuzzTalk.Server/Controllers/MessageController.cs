using BuzzTalk.Server.Hubs;
using BuzzTalk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using AutoMapper;
using BuzzTalk.Business.Services;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Data.Models;

namespace BuzzTalk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHubContext<BuzzChatHub, IbuzzChatHub> _hubContext;

        public MessageController(IMessageService messageService, IMapper mapper, IConfiguration config, IHubContext<BuzzChatHub, IbuzzChatHub> hubContext)
        {
            _messageService = messageService;
            _mapper = mapper;
            _config = config;
            _hubContext = hubContext;
        }
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(MessageHub message)
        {
            var user = this.User;
            var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            var messageModel = _mapper.Map<MessageDto>(message);
            var res = await _messageService.SendMessage(messageModel);
            if (!res.Item1)
            {
                return BadRequest("Message not sent");
            }
            message= _mapper.Map<MessageHub>(res.Item3);
            var Receiver = BuzzChatHub._connectedUsers.FirstOrDefault(x => x.Key == message.ToId).Value;
            if (Receiver!=null)
            {
                await _hubContext.Clients.Client(Receiver.ConnectionId).NewMesseageRecived(message);
            }
            return Ok(message);
        }
        [HttpPost("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages(GetmessageModel getmessage)
        {
            var messages = await _messageService.GetAllMessages(getmessage.FromId,getmessage.ToId);
            if (messages != null)
            {
                return Ok(messages);
            }
            return NotFound("No messages found");
        }
        [HttpPost("MarkRead")]
        public async Task<IActionResult> MarkRead(GetmessageModel getmessage)
        {
            var messages = await _messageService.MarkRead(getmessage.FromId, getmessage.ToId);
            if (messages != null)
            {
                return Ok(messages);
            }
            var Receiver = BuzzChatHub._connectedUsers.FirstOrDefault(x => x.Key == getmessage.ToId).Value;
            if (Receiver != null)
            {
                await _hubContext.Clients.Client(Receiver.ConnectionId).MarkMessagesRead(_mapper.Map<MessageHub>(messages));
            }
            return NotFound("No messages found");
        }
    }
}
