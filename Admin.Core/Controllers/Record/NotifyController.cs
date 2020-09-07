using System;
using System.Threading.Tasks;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Hubs;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.Notify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Admin.Core.Controllers.Record
{
    public class NotifyController:AreaController
    {
        private readonly INotifyService _notifyService;
        private readonly IUser _user;
        private readonly IHubContext<ChatHub> _hubContext;
        public NotifyController(INotifyService notifyService
            , IUser user
            , IHubContext<ChatHub> hubContext)
        {
            _user = user;
            _notifyService = notifyService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// 根据用户主键获取用户消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetCount()
        {
            return await _notifyService.GetCountAsync(_user.Id);
        }

        /// <summary>
        /// 根据用户主键获取分页信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPage(PageInput<NotifyEntity> input)
        {
            return await _notifyService.GetPageAsync(input, _user.Id);
        }

        /// <summary>
        /// 读取通知
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> ReadNotify(long id)
        {
            var data = await _notifyService.ReadNotifyAsync(id);
            if (RedisHelper.HExists("signalR", _user.Id.ToString()))
            {
                await _hubContext.Clients.Client(RedisHelper.HGet("signalR", _user.Id.ToString())).SendAsync("Show", "信息刷新", "");
            }
            return data;
        }

        [HttpGet]
        [AllowAnonymous]
        public IResponseOutput GetDictionary()
        {
            var data = new { count = RedisHelper.HLen("signalR"), str = RedisHelper.HKeys("signalR").ToString() + RedisHelper.HVals("signalR").ToString() };
            return ResponseOutput.Ok(data);
        }
    }
}
