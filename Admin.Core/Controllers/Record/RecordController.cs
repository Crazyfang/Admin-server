using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.Record;
using Admin.Core.Service.Record.Record.Input;
using Admin.Core.Service.Record.RecordFileType.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Admin.Core.Controllers.Record
{
    public class RecordController:AreaController
    {
        private readonly IRecordService _recordService;

        public RecordController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordService.GetAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPage(PageInput<RecordEntity> input)
        {
            return await _recordService.PageAsync(input);
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _recordService.DeleteAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(JObject obj)
        {
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordAddInput>(obj["record"].ToString());
            var recordFileTypeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeOutput>>(obj["fileList"].ToString());
            return await _recordService.AddAsync(input, recordFileTypeList);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Update(JObject obj)
        {
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordUpdateInput>(obj["record"].ToString());
            var recordFileTypeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeOutput>>(obj["fileList"].ToString());
            return await _recordService.UpdateAsync(input, recordFileTypeList);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetBasicInfo(long id)
        {
            return await _recordService.GetRecordInfoAsync(id);
        }
    }
}
