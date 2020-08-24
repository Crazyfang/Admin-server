using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Record
{
    public class RecordHistoryController:AreaController
    {
        private readonly IRecordHistoryService _recordHistoryService;

        public RecordHistoryController(IRecordHistoryService recordHistoryService)
        {
            _recordHistoryService = recordHistoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordHistoryService.GetAsync(id);
        }
    }
}
