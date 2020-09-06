using Admin.Core.Common.Auth;
using Admin.Core.Common.Helpers;
using Admin.Core.Repository.Admin;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Admin.Core.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUser _user;
        private readonly IUserRepository _userRepository;
        //public static Dictionary<long, string> _connections = new Dictionary<long, string>();

        public ChatHub(IUser user, IUserRepository userRepository)
        {
            _user = user;
            _userRepository = userRepository;
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

                RedisHelper.HSet("signalR", userId.ToString(), Context.ConnectionId);
            }
                       
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            var id = Context.User?.FindFirst(ClaimAttributes.UserId);
            long userId = 0;
            if (id != null && id.Value.NotNull())
            {
                userId = id.Value.ToLong();
            }
            if(userId != 0)
            {
                RedisHelper.HDel("signalR", userId.ToString());
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