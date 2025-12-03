using AutoMapper;
using BuzzTalk.Business.Dtos;
using BuzzTalk.Business.Services;
using BuzzTalk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BuzzTalk.Server.Hubs
{

    public interface IbuzzChatHub
    {
        Task UserConnected(UserModelHub user);
        Task ConnectedUsers(IEnumerable<UserModelHub> users);
        Task UserDisconnected(UserModelHub user);
        Task NewUserSignIn(UserModelHub user);
        Task NewMessageReceive(MessageHub message);
        Task MarkMessagesRead(MessageHub message);
        Task NewGroupCreated(GroupModel group);
    }
    [Authorize]
    public class BuzzChatHub : Hub<IbuzzChatHub>
    {
        public static readonly IDictionary<int, UserModelHub> _connectedUsers = new Dictionary<int, UserModelHub>();
        public static readonly List<int> _connectedUserId = new List<int>();
        public static readonly IDictionary<int, string> _activeGroups = new Dictionary<int, string>();
        private readonly IMessageService _messageService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;


        public BuzzChatHub(IMessageService messageService, IMapper mapper, IGroupService groupService,INotificationService notificationService)
        {
            _messageService = messageService;
            _mapper = mapper;
            _groupService = groupService;
            _notificationService = notificationService;
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
                _connectedUserId.Add(user.Id);
            }
            await Clients.Others.UserConnected(user);
        }
        public async Task<List<GroupModel>> GetGroups()
        {
            var userId = int.Parse(this.Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var res = await _groupService.GetAllGroup(userId);
            if (res != null)
            {
                var realGroup = res.Where(x => x.Name != null).ToList();
                var user = _connectedUsers.FirstOrDefault(x => x.Key ==userId).Value;
                if (user != null&& realGroup!=null) {
                    foreach (var group in realGroup)
                    {
                        if (!_activeGroups.ContainsKey(group.Id))
                        {
                            _activeGroups.Add(group.Id, group.Guid);
                        }
                        if (_activeGroups.ContainsKey(group.Id))
                        {
                            var activeGroup = _activeGroups.FirstOrDefault(x => x.Key == group.Id).Value;
                            if (activeGroup != null)
                            {
                                await Groups.AddToGroupAsync(user.ConnectionId, group.Guid);
                            }
                        }
                    }
                }
            }
            return _mapper.Map<List<GroupModel>>(res);
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user = this.Context.User;
            var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (_connectedUsers.ContainsKey(userId))
            {
                _connectedUsers.Remove(userId);
                _connectedUserId.Remove(userId);
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
            var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            var messageModel = _mapper.Map<MessageDto>(message);
            var res = await _messageService.SendMessage(messageModel);
            var Receiver = _connectedUsers.FirstOrDefault(x => x.Key == message.ToId).Value;
            var title = "";
            if (res.Item3.GroupId != messageModel.GroupId)
            {
                var group = await _groupService.GetGroup(res.Item3.GroupId ?? 0);
                if (group != null && Receiver != null)
                {
                    await Clients.Client(Receiver.ConnectionId).NewGroupCreated(_mapper.Map<GroupModel>(group));
                }
            }

            message = _mapper.Map<MessageHub>(res.Item3);
            if (_activeGroups.ContainsKey((int)message.GroupId))
            {
                title =await GetNotificationTitle(userId,message.GroupId);
                await Clients.OthersInGroup(_activeGroups.FirstOrDefault(x=>x.Key==message.GroupId).Value).NewMessageReceive(message);
            }
            else if(Receiver != null)
            {
                title = await GetNotificationTitle(userId);
                await Clients.Client(Receiver.ConnectionId).NewMessageReceive(message);
            }
            var notification = new NotificationDto()
            {
                GroupId = (int)message.GroupId,
                Description = message.Content,
                Title = title,
                UserId= userId,
            };
            await _notificationService.SendMessage(notification,_connectedUserId);
            return message;
        }
        private async Task<string> GetNotificationTitle(int userId, int? groupId)
        {
            var group =await _groupService.GetGroup((int)groupId);
            var sender = _connectedUsers.FirstOrDefault(x => x.Key == userId).Value;
            return $"{group.Name} : {sender.Name}";
        }
        private async Task<string> GetNotificationTitle(int userId)
        {
            var sender = _connectedUsers.FirstOrDefault(x => x.Key == userId).Value;
            return $"{sender.Name}";
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
