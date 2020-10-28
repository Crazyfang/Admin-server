using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordFile.Input;
using Admin.Core.Service.Record.RecordFile.Output;

namespace Admin.Core.Service.Record.RecordFile
{
    public partial interface IRecordFileService
    {
        /// <summary>
        /// 根据文件主键获取实体
        /// </summary>
        /// <param name="id">文件主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetAsync(long id);

        /// <summary>
        /// 根据档案类型主键获取档案文件集合(档案类型编辑界面使用)
        /// </summary>
        /// <param name="id">档案类型主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetRecordFileListAsync(long id);

        /// <summary>
        /// 添加档案文件
        /// </summary>
        /// <param name="input">档案文件信息</param>
        /// <returns></returns>
        Task<IResponseOutput> AddAsync(RecordFileAddInput input);

        /// <summary>
        /// 更新档案文件
        /// </summary>
        /// <param name="input">档案文件信息</param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAsync(RecordFileUpdateInput input);

        /// <summary>
        /// 根据主键删除档案文件
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(long id);

        /// <summary>
        /// 根据档案类型主键获取档案文件集合(添加界面使用)
        /// </summary>
        /// <param name="id">档案类型主键</param>
        /// <returns></returns>
        Task<List<AddRecordFileOutput>> GetRecordFileByRecordTypeIdAsync(long id);
    }
}
