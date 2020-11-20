using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Antimoney.File.Input;

namespace Admin.Core.Service.Antimoney.File
{
    public partial interface IFileService
    {
        /// <summary>
        /// 添加合同下的文件
        /// </summary>
        /// <param name="input">文件列表</param>
        /// <returns></returns>
        Task<IResponseOutput> AddFileAsync(IList<FileAddOrEditInput> input);
    }
}
