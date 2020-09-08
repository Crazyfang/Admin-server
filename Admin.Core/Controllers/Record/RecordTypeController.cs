using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordType;
using Admin.Core.Service.Record.RecordType.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Record
{
    public class RecordTypeController : AreaController
    {
        private readonly IRecordTypeService _recordTypeService;

        public RecordTypeController(IRecordTypeService recordTypeService)
        {
            _recordTypeService = recordTypeService;
        }

        /// <summary>
        /// 档案类别获取
        /// </summary>
        /// <param name="id">档案类别主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordTypeService.GetAsync(id);
        }

        /// <summary>
        /// 档案类别全部获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetAll()
        {
            return await _recordTypeService.GetAllRecordTypeAsync();
        }

        /// <summary>
        /// 档案类别添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(RecordTypeAddInput input)
        {
            return await _recordTypeService.AddRecordTypeAsync(input);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Update(RecordTypeUpdateInput input)
        {
            return await _recordTypeService.UpdateRecordTypeAsync(input);
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _recordTypeService.DeleteAsync(id);
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _recordTypeService.SoftDeleteAsync(id);
        }
    }
}