using Admin.Core.Common.Attributes;
using Admin.Core.Common.Configs;
using Admin.Core.Common.Output;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = Admin.Core.Common.Files.FileInfo;

namespace Admin.Core.Common.Helpers
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    [SingleInstance]
    public class UploadHelper
    {
        /// <summary>
        /// 上传多文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="config"></param>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IResponseOutput<List<FileInfo>>> UploadMultiAsync(List<IFormFile> files, FileUploadConfig config, object args, CancellationToken cancellationToken = default)
        {
            var res = new ResponseOutput<List<FileInfo>>();
            var msg = "";
            var count = 0;
            var gather = new List<FileInfo>();

            if (files == null || files.Count < 1)
            {
                return res.NotOk("请上传文件！");
            }
            foreach (var file in files)
            {
                //格式限制
                if (!config.ContentType.Contains(file.ContentType))
                {
                    msg += $"{file.FileName}文件格式错误<br>";
                }

                //大小限制
                if (!(file.Length <= config.MaxSize))
                {
                    msg += $"{file.FileName}文件格式错误<br>";
                }

                var fileInfo = new FileInfo(file.FileName, file.Length)
                {
                    UploadPath = config.UploadPath,
                    RequestPath = config.RequestPath
                };

                var dateTimeFormat = config.DateTimeFormat.NotNull() ? DateTime.Now.ToString(config.DateTimeFormat) : "";
                var format = config.Format.NotNull() ? StringHelper.Format(config.Format, args) : "";
                fileInfo.RelativePath = Path.Combine(dateTimeFormat, format).ToPath();

                if (!Directory.Exists(fileInfo.FileDirectory))
                {
                    Directory.CreateDirectory(fileInfo.FileDirectory);
                }

                fileInfo.SaveName = $"{IdWorkerHelper.GenId64()}.{fileInfo.Extension}";

                await SaveAsync(file, fileInfo.FilePath, cancellationToken);
                count += 1;
                gather.Add(fileInfo);
            }

            msg = msg.Insert(0, $"上传成功{count}张图片<br>");
            return res.Ok(gather, msg);
        }

        /// <summary>
        /// 上传单文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="config"></param>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IResponseOutput<FileInfo>> UploadAsync(IFormFile file, FileUploadConfig config, object args, CancellationToken cancellationToken = default)
        {
            var res = new ResponseOutput<FileInfo>();

            if (file == null || file.Length < 1)
            {
                return res.NotOk("请上传文件！");
            }

            //格式限制
            if (!config.ContentType.Contains(file.ContentType))
            {
                return res.NotOk("文件格式错误");
            }

            //大小限制
            if (!(file.Length <= config.MaxSize))
            {
                return res.NotOk("文件过大");
            }

            var fileInfo = new FileInfo(file.FileName, file.Length)
            {
                UploadPath = config.UploadPath,
                RequestPath = config.RequestPath
            };

            var dateTimeFormat = config.DateTimeFormat.NotNull() ? DateTime.Now.ToString(config.DateTimeFormat) : "";
            var format = config.Format.NotNull() ? StringHelper.Format(config.Format, args) : "";
            fileInfo.RelativePath = Path.Combine(dateTimeFormat, format).ToPath();

            if (!Directory.Exists(fileInfo.FileDirectory))
            {
                Directory.CreateDirectory(fileInfo.FileDirectory);
            }

            fileInfo.SaveName = $"{IdWorkerHelper.GenId64()}.{fileInfo.Extension}";

            await SaveAsync(file, fileInfo.FilePath, cancellationToken);

            return res.Ok(fileInfo);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveAsync(IFormFile file, string filePath, CancellationToken cancellationToken = default)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }
        }
    }
}
