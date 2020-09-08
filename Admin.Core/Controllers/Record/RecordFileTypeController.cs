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
    /// <summary>
    /// 信贷档案文件类别
    /// </summary>
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

        /// <summary>
        /// 档案文件类别获取
        /// </summary>
        /// <param name="id">档案文件类别主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordFileTypeService.GetAsync(id);
        }

        /// <summary>
        /// 获取档案文件类别下属的档案文件
        /// </summary>
        /// <param name="id">档案类别主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetListByRecTypeId(long id)
        {
            return await _recordFileTypeService.GetRecordFileListAsync(id);
        }

        /// <summary>
        /// 档案文件类别添加
        /// </summary>
        /// <param name="input">档案文件类别添加类</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(RecordFileTypeAddInput input)
        {
            return await _recordFileTypeService.AddRecordFileTypeAsync(input);
        }

        /// <summary>
        /// 档案文件类别更新
        /// </summary>
        /// <param name="input">档案文件类别更新类</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Update(RecordFileTypeUpdateInput input)
        {
            return await _recordFileTypeService.UpdateRecordFileTypeAsync(input);
        }

        /// <summary>
        /// 档案文件类别删除
        /// </summary>
        /// <param name="id">档案文件类别主键</param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _recordFileTypeService.DeleteRecordFileTypeAsync(id);
        }

        /// <summary>
        /// 档案编辑界面原有文件信息返回
        /// </summary>
        /// <param name="id">档案类型</param>
        /// <param name="recordId">档案主键</param>
        /// <returns></returns>
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

        /// <summary>
        /// 档案添加界面返回文件类别及归属文件
        /// </summary>
        /// <param name="id">档案类别</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddRecordPageList(long id)
        {
            var recordFileTypeList = await _recordFileTypeService.AddRecordPageListAsync(id);

            return ResponseOutput.Ok(recordFileTypeList);
        }
    }
}
