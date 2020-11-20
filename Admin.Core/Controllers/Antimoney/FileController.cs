using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Attributes;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Output;
using Admin.Core.Service.Antimoney.File;
using Admin.Core.Service.Antimoney.File.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Admin.Core.Controllers.Antimoney
{
    public class FileController : AreaController
    {
        private readonly IUser _user;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;
        private readonly IFileService _fileService;
        public FileController(
            IUser user,
            IOptionsMonitor<UploadConfig> uploadConfig,
            UploadHelper uploadHelper,
            IFileService fileService)
        {
            _user = user;
            _uploadConfig = uploadConfig.CurrentValue;
            _uploadHelper = uploadHelper;
            _fileService = fileService;
        }

        /// <summary>
        /// 添加或编辑合同下文件
        /// </summary>
        /// <param name="input">文件列表</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(List<FileAddOrEditInput> input)
        {
            return await _fileService.AddFileAsync(input);
        }

        /// <summary>
        /// 上传文件图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Login]
        public async Task<IResponseOutput> FileImageUpload([FromForm] IFormFile file)
        {
            var config = _uploadConfig.Antimoney;
            var res = await _uploadHelper.UploadAsync(file, config, new { Directory = "Image" });
            if (res.Success)
            {
                return ResponseOutput.Ok(res.Data.FileRelativePath);
            }

            return ResponseOutput.NotOk("上传失败！");
        }
    }
}
