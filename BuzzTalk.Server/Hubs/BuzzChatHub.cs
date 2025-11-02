using BuzzTalk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Business.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BuzzTalk.Server.Hubs
{
    
    public interface IbuzzChatHub
    {
        Task UserConnected(UserModelHub user);
        Task ConnectedUsers(IEnumerable<UserModelHub> users);
        Task UserDisconnected(UserModelHub user);
        Task  NewUserSignIn(UserModelHub user);
        Task NewMesseageRecived(MessageHub message);
        Task MarkMessagesRead(MessageHub message);
    }
    public class BuzzChatHub : Hub<IbuzzChatHub>
    {
        public static readonly IDictionary<int,UserModelHub> _connectedUsers = new Dictionary<int,UserModelHub>();
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public BuzzChatHub(IMessageService messageService,IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }
        public override Task OnConnectedAsync()
        {
            var user = this.Context.User;
            var userName = user.FindFirst(ClaimTypes.Name);
            return base.OnConnectedAsync();
        }
        public async Task ConnectUser(UserModelHub user)
        {
            await Clients.Caller.ConnectedUsers(_connectedUsers.Values.Where(x => x.Id != user.Id).ToList());
            if (!_connectedUsers.ContainsKey(user.Id))
            {
                user.ConnectionId = Context.ConnectionId;
                _connectedUsers.ContainsKey(user.Id);
                _connectedUsers.Add(user.Id, user);
                Console.WriteLine(_connectedUsers);
            }
           await Clients.Others.UserConnected(user);
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user = this.Context.User;
            var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (_connectedUsers.ContainsKey(userId))
            {
                _connectedUsers.Remove(userId);
            }
            var userModel = new UserModelHub
            {
                Id = userId,
                ConnectionId = Context.ConnectionId
            };
            Clients.Others.UserDisconnected(userModel);

            return base.OnDisconnectedAsync(exception);
        }
        public async Task<MessageHub> SendMessage(MessageHub message)
        {
            var user = this.Context.User;
            var userId =  int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            var messageModel = _mapper.Map<MessageDto>(message);
            var res = await _messageService.SendMessage(messageModel);

            message = _mapper.Map<MessageHub>(res.Item3);
            var Receiver = _connectedUsers.FirstOrDefault(x => x.Key == message.ToId).Value;
            if (Receiver != null)
            {
                await Clients.Client(Receiver.ConnectionId).NewMesseageRecived(message);
            }
            return message;
        }
        //public async Task<List<MessageHub>> MarkRead(MessageHub getmessage)
        //{
        //    var messages = await _messageService.MarkRead(getmessage.FromId, getmessage.ToId);
        //    if (messages != null)
        //    {
        //        return messages;
        //    }
        //    var Receiver = BuzzChatHub._connectedUsers.FirstOrDefault(x => x.Key == getmessage.ToId).Value;
        //    if (Receiver != null)
        //    {
        //        await Clients.Client(Receiver.ConnectionId).MarkMessagesRead(_mapper.Map<MessageHub>(messages));
        //    }
        //    return
        //}

    }
}
