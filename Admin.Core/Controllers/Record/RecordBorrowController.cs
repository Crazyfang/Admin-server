using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Hubs;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.Notify;
using Admin.Core.Service.Record.RecordBorrow;
using Admin.Core.Service.Record.RecordBorrow.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Admin.Core.Controllers.Record
{
    public class RecordBorrowController:AreaController
    {
        private readonly IRecordBorrowService _recordBorrowService;
        private readonly INotifyService _notifyService;
        private readonly IHubContext<ChatHub> _hubContext;
        public RecordBorrowController(IRecordBorrowService recordBorrowService
            , INotifyService notifyService
            , IHubContext<ChatHub> hubContext)
        {
            _recordBorrowService = recordBorrowService;
            _notifyService = notifyService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// 添加档案借调阅
        /// </summary>
        /// <param name="input">申请档案借阅基本信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddRecordBorrow(RecordBorrowAddInput input)
        {
           return await _recordBorrowService.BorrowOrReadAsync(input);
        }

        /// <summary>
        /// 取消档案借调阅
        /// </summary>
        /// <param name="id">借调阅主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> CancelRecordBorrow(long id)
        {
            return await _recordBorrowService.CancelBorrowOrReadAsync(id);
        }

        /// <summary>
        /// 获取当前登录用户的借调阅列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetBorrowList()
        {
            return await _recordBorrowService.GetBorrowListAsync();
        }

        /// <summary>
        /// 获取借调阅审核分页
        /// </summary>
        /// <param name="input">分页参数及筛选条件</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetVerifyList(PageInput<RecordBorrowEntity> input)
        {
            return await _recordBorrowService.GetVerifyListAsync(input);
        }

        /// <summary>
        /// 获取借调阅具体信息
        /// </summary>
        /// <param name="id">借调阅主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetBorrowDetail(long id)
        {
            return await _recordBorrowService.GetBorrowDetailAsync(id);
        }

        /// <summary>
        /// 借调阅审核
        /// </summary>
        /// <param name="input">借调阅信息</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> VerifyBorrow(RecordBorrowVerifyInput input)
        {
            var data = await _recordBorrowService.VerifyBorrowAsync(input);

            var user = await _recordBorrowService.GetUserByBorrowId(input.Id);

            if(input.VerifyType == 0)
            {
                if (RedisHelper.HExists("signalR", user.Id.ToString()))
                {
                    await _hubContext.Clients.Client(RedisHelper.HGet("signalR", user.Id.ToString())).SendAsync("Show", "信息刷新", $"您有一份借调阅审核被拒绝");
                }
            }
            else
            {
                if (RedisHelper.HExists("signalR", user.Id.ToString()))
                {
                    await _hubContext.Clients.Client(RedisHelper.HGet("signalR", user.Id.ToString())).SendAsync("Show", "信息刷新", $"您有一份借调阅审核通过");
                }
            }
            
            return data;
        }

        /// <summary>
        /// 获取借调阅归还分页
        /// </summary>
        /// <param name="input">分页信息及筛选条件</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetReturnPage(PageInput<RecordBorrowEntity> input)
        {
            return await _recordBorrowService.GetReturnPageAsync(input);
        }

        /// <summary>
        /// 借调阅归还
        /// </summary>
        /// <param name="id">借调阅主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> ReturnRecord(long id)
        {
            return await _recordBorrowService.ReturnRecordAsync(id);
        }
    }

}
