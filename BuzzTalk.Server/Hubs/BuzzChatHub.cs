using BuzzTalk.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

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
        public BuzzChatHub()
        {

        }
        public override Task OnConnectedAsync()
        {
            //var user = this.Context.User;
            //var userName = user.FindFirst(ClaimTypes.Name);
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

    }
}
