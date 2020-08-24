using Admin.Core.Common.Auth;
using Admin.Core.Common.Helpers;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.User;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Admin.Core.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUser _user;
        private readonly IUserRepository _userRepository;

        public ChatHub(IUser user, IUserRepository userRepository)
        {
            _user = user;
            _userRepository = userRepository;
        }

        public override Task OnConnectedAsync()
        {
            var test2 = Context.UserIdentifier;
            var test3 = Context.User.FindFirst(ClaimAttributes.UserId);
            var test = Context.GetHttpContext().User.FindFirst(ClaimAttributes.UserId).Value.ToLong();
            var data = _userRepository.Select.WhereDynamic(test).IncludeMany(i => i.Roles).ToOne();
            var sign = false;
            foreach(var item in data.Roles)
            {
                if(item.Name == "系统管理员" || item.Name == "文档管理员")
                {
                    sign = true;
                }
            }
            if (sign)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }
            
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            ConsoleHelper.WriteInfoLine($"{Context.ConnectionId}下线");
            return base.OnDisconnectedAsync(exception);
        }

        public Task SendMsg(string info)
        {  //这里的Show代表是客户端的方法，具体可以细看SignalR的说明
           
           return Clients.Group("Admin").SendAsync("Show", info, Context.ConnectionId);
        }
    }
}