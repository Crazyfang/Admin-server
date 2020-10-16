using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.CheckedRecordFile.Input;

namespace Admin.Core.Service.Record.CheckedRecordFile
{
    public partial interface ICheckedRecordFileService
    {
        /// <summary>
        /// 获取选中文件
        /// </summary>
        /// <param name="id">选中文件主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetAsync(long id);

        /// <summary>
        /// 获取指定档案和档案文件类别归属的选中文件
        /// </summary>
        /// <param name="id">选中档案类别</param>
        /// <param name="recordId">档案主键</param>
        /// <returns></returns>
        Task<List<CheckedRecordFileInput>> GetCheckedRecordFileAsync(long id, long recordId);

        /// <summary>
        /// 修改档案文件的状态
        /// </summary>
        /// <param name="status">状态字</param>
        /// <param name="recordId">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> ChangeFileStatusAsync(int status, long recordId);
    }
}
