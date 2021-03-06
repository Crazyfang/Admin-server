﻿using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordFile;
using Admin.Core.Service.Record.RecordFile.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Record
{
    /// <summary>
    /// 信贷档案文件
    /// </summary>
    public class RecordFileController:AreaController
    {
        private readonly IRecordFileService _recordFileService;
        public RecordFileController(IRecordFileService recordFileService)
        {
            _recordFileService = recordFileService;
        }

        /// <summary>
        /// 根据id获取特定的档案文件条目
        /// </summary>
        /// <param name="id">档案文件id</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordFileService.GetAsync(id);
        }

        /// <summary>
        /// 根据档案文件类型Id获取档案文件列表
        /// </summary>
        /// <param name="id">档案文件类型Id</param>
        /// <returns>档案文件列表</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetListByRecFileTypeId(long id)
        {
            return await _recordFileService.GetRecordFileListAsync(id);
        }

        /// <summary>
        /// 档案文件类别下属文件添加
        /// </summary>
        /// <param name="input">档案文件添加类</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(RecordFileAddInput input)
        {
            return await _recordFileService.AddAsync(input);
        }

        /// <summary>
        /// 档案文件类别下属文件更新
        /// </summary>
        /// <param name="input">档案文件更新类</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Update(RecordFileUpdateInput input)
        {
            return await _recordFileService.UpdateAsync(input);
        }

        /// <summary>
        /// 档案文件类别下属文件删除
        /// </summary>
        /// <param name="id">档案文件删除类</param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _recordFileService.DeleteAsync(id);
        }
    }
}
