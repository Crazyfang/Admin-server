using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Record
{
    /// <summary>
    /// 信贷档案历史
    /// </summary>
    public class RecordHistoryController:AreaController
    {
        private readonly IRecordHistoryService _recordHistoryService;

        public RecordHistoryController(IRecordHistoryService recordHistoryService)
        {
            _recordHistoryService = recordHistoryService;
        }

        /// <summary>
        /// 档案历史查询
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordHistoryService.GetAsync(id);
        }
    }
}
