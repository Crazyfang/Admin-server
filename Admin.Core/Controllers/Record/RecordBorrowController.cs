using System;
using System.Threading.Tasks;
using Admin.Core.Common.Helpers;
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
using Newtonsoft.Json.Linq;

namespace Admin.Core.Controllers.Record
{
    public class RecordBorrowController:AreaController
    {
        private readonly IRecordBorrowService _recordBorrowService;
        private readonly INotifyService _notifyService;
        private readonly SignalRDictionary _signalRDictionary;
        private readonly IHubContext<ChatHub> _hubContext;
        public RecordBorrowController(IRecordBorrowService recordBorrowService
            , INotifyService notifyService
            , SignalRDictionary signalRDictionary
            , IHubContext<ChatHub> hubContext)
        {
            _recordBorrowService = recordBorrowService;
            _notifyService = notifyService;
            _signalRDictionary = signalRDictionary;
            _hubContext = hubContext;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddRecordBorrow(RecordBorrowAddInput input)
        {
           return await _recordBorrowService.BorrowOrReadAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> CancelRecordBorrow(long id)
        {
            return await _recordBorrowService.CancelBorrowOrReadAsync(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetBorrowList()
        {
            return await _recordBorrowService.GetBorrowListAsync();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetVerifyList(PageInput<RecordBorrowEntity> input)
        {
            return await _recordBorrowService.GetVerifyListAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetBorrowDetail(long id)
        {
            return await _recordBorrowService.GetBorrowDetailAsync(id);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> VerifyBorrow(RecordBorrowVerifyInput input)
        {
            var data = await _recordBorrowService.VerifyBorrowAsync(input);

            var user = await _recordBorrowService.GetUserByBorrowId(input.Id);

            if(input.VerifyType == 0)
            {
                if (_signalRDictionary.connections.ContainsKey(user.Id))
                {
                    await _hubContext.Clients.Client(_signalRDictionary.connections[user.Id]).SendAsync("Show", "信息刷新", $"您有一份借调阅审核被拒绝");
                }
            }
            else
            {
                if (_signalRDictionary.connections.ContainsKey(user.Id))
                {
                    await _hubContext.Clients.Client(_signalRDictionary.connections[user.Id]).SendAsync("Show", "信息刷新", $"您有一份借调阅审核通过");
                }
            }
            
            return data;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetReturnPage(PageInput<RecordBorrowEntity> input)
        {
            return await _recordBorrowService.GetReturnPageAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> ReturnRecord(long id)
        {
            return await _recordBorrowService.ReturnRecordAsync(id);
        }
    }

}
