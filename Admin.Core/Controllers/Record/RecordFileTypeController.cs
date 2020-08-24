using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.CheckedRecordFile;
using Admin.Core.Service.Record.Record;
using Admin.Core.Service.Record.RecordFile;
using Admin.Core.Service.Record.RecordFileType;
using Admin.Core.Service.Record.RecordFileType.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Record
{
    public class RecordFileTypeController:AreaController
    {
        private readonly IRecordFileTypeService _recordFileTypeService;
        private readonly ICheckedRecordFileService _checkedRecordFileService;
        private readonly IRecordService _recordService;
        public RecordFileTypeController(IRecordFileTypeService recordFileTypeService
            , ICheckedRecordFileService checkedRecordFileService
            , IRecordFileService recordFileService
            , IRecordService recordService)
        {
            _recordFileTypeService = recordFileTypeService;
            _checkedRecordFileService = checkedRecordFileService;
            _recordService = recordService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordFileTypeService.GetAsync(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetListByRecTypeId(long id)
        {
            return await _recordFileTypeService.GetRecordFileListAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(RecordFileTypeAddInput input)
        {
            return await _recordFileTypeService.AddRecordFileTypeAsync(input);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Update(RecordFileTypeUpdateInput input)
        {
            return await _recordFileTypeService.UpdateRecordFileTypeAsync(input);
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _recordFileTypeService.DeleteRecordFileTypeAsync(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> UpdateRecordPageList(long id, long recordId)
        {
            var record = await _recordService.GetRecordTypeAsync(recordId);
            if (id == record)
            {
                var recordFileTypeList = await _recordFileTypeService.UpdateRecordPageListAsync(id, recordId);

                return ResponseOutput.Ok(recordFileTypeList);
            }
            else
            {
                var recordFileTypeList = await _recordFileTypeService.AddRecordPageListAsync(id);

                return ResponseOutput.Ok(recordFileTypeList);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddRecordPageList(long id)
        {
            var recordFileTypeList = await _recordFileTypeService.AddRecordPageListAsync(id);

            return ResponseOutput.Ok(recordFileTypeList);
        }
    }
}
