using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.RecordBorrow;
using Admin.Core.Service.Record.RecordBorrow.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Admin.Core.Controllers.Record
{
    public class RecordBorrowController:AreaController
    {
        private readonly IRecordBorrowService _recordBorrowService;
        public RecordBorrowController(IRecordBorrowService recordBorrowService)
        {
            _recordBorrowService = recordBorrowService;
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
            return await _recordBorrowService.VerifyBorrowAsync(input);
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
