using BuzzTalk.Business.Dtos;
using FirebaseAdmin.Messaging;

namespace BuzzTalk.Business.Services
{
    public interface INotificationService
    {
        Task SendMessage(NotificationDto notification,List<int> connectedUser);
    }
    public class NotificationService : INotificationService
    {
        private readonly IGroupService _groupService;
        public NotificationService(IGroupService groupService)
        {
            _groupService = groupService;
        }
        public async Task SendMessage(NotificationDto notification, List<int> connectedUser)
        {
            try
            {
                var tokens = await _groupService.GetGroupAllUserToken(notification.GroupId,notification.UserId);
                IEnumerable<Message> messages = tokens.Where(x=>!connectedUser.Contains(x.Item1)&&x.Item2!=null).Select(x => new Message()
                {
                    Token = x.Item2,
                    Notification = new Notification()
                    {
                        Title = notification.Title,
                        Body = notification.Description
                    }
                });
                if (messages.Count() < 1) return;
                FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
                BatchResponse response = await messaging.SendEachAsync(messages);
                var res = response.SuccessCount;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
    }
}
