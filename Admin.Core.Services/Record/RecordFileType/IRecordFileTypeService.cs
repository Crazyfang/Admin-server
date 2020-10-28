using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordFileType.Input;
using Admin.Core.Service.Record.RecordFileType.Output;

namespace Admin.Core.Service.Record.RecordFileType
{
    public partial interface IRecordFileTypeService
    {
        /// <summary>
        /// 根据档案类型主键获取档案类型
        /// </summary>
        /// <param name="id">档案类型主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetAsync(long id);

        /// <summary>
        /// 根据档案类型主键获取档案类型集合
        /// </summary>
        /// <param name="id">档案类别主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetRecordFileListAsync(long id);

        /// <summary>
        /// 添加档案文件类型
        /// </summary>
        /// <param name="input">档案类型内容</param>
        /// <returns></returns>
        Task<IResponseOutput> AddRecordFileTypeAsync(RecordFileTypeAddInput input);

        /// <summary>
        /// 更新档案文件类型
        /// </summary>
        /// <param name="input">档案类型内容</param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateRecordFileTypeAsync(RecordFileTypeUpdateInput input);

        /// <summary>
        /// 根据档案文件类型主键删除档案类型
        /// </summary>
        /// <param name="id">档案类型主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteRecordFileTypeAsync(long id);

        /// <summary>
        /// 返回档案编辑界面的档案文件类型及其下属档案文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recordId">档案主键</param>
        /// <returns></returns>
        Task<List<RecordFileTypeUpdateOutput>> UpdateRecordPageListAsync(long id, long recordId);

        /// <summary>
        /// 返回档案添加界面的档案类型及其下属档案文件
        /// </summary>
        /// <param name="id">档案类型主键</param>
        /// <returns></returns>
        Task<List<RecordFileTypeAddOutput>> AddRecordPageListAsync(long id);
    }
}
