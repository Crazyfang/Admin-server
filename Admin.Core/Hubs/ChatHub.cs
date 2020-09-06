using Admin.Core.Common.Auth;
using Admin.Core.Common.Helpers;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.User;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Core.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUser _user;
        private readonly IUserRepository _userRepository;
        private readonly SignalRDictionary _signalRDictionary;
        //public static Dictionary<long, string> _connections = new Dictionary<long, string>();

        public ChatHub(IUser user, IUserRepository userRepository, SignalRDictionary signalRDictionary)
        {
            _user = user;
            _userRepository = userRepository;
            _signalRDictionary = signalRDictionary;
        }

        public override async Task OnConnectedAsync()
        {
            var id = Context.User?.FindFirst(ClaimAttributes.UserId);
            long userId = 0;
            if (id != null && id.Value.NotNull())
            {
                userId = id.Value.ToLong();
            }
            if(userId != 0)
            {
                //var test = Context.GetHttpContext().User.FindFirst(ClaimAttributes.UserId).Value.ToLong();
                var data = _userRepository.Select.WhereDynamic(userId).IncludeMany(i => i.Roles).ToOne();
                var sign = false;
                foreach (var item in data.Roles)
                {
                    if (item.Name == "系统管理员" || item.Name == "文档管理员")
                    {
                        sign = true;
                    }
                }
                if (sign)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
                }

                if (!_signalRDictionary.connections.ContainsKey(userId))
                {
                    _signalRDictionary.connections.Add(userId, Context.ConnectionId);
                }
                else
                {
                    if (Context.ConnectionId != _signalRDictionary.connections[userId])
                    {
                        _signalRDictionary.connections[userId] = Context.ConnectionId;
                    }
                }
            }
                       
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            var key = _signalRDictionary.connections.Where(i => i.Value == Context.ConnectionId).Select(i => i.Key).FirstOrDefault();
            if (key != 0)
            {
                _signalRDictionary.connections.Remove(key);
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admin");
            ConsoleHelper.WriteInfoLine($"{Context.ConnectionId}下线");
            await base.OnDisconnectedAsync(exception);
        }

        public Task SendMsg(string info)
        {  //这里的Show代表是客户端的方法，具体可以细看SignalR的说明
           
           return Clients.Group("Admin").SendAsync("Show", info, Context.ConnectionId);
        }
    }
}