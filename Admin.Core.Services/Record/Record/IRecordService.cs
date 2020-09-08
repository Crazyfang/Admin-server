using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.CheckedRecordFile.Input;
using Admin.Core.Service.Record.Record.Input;
using Admin.Core.Service.Record.Record.Output;
using Admin.Core.Service.Record.RecordFileType.Output;

namespace Admin.Core.Service.Record.Record
{
    public partial interface IRecordService
    {
        /// <summary>
        /// 获取档案
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetAsync(long id);

        /// <summary>
        /// 档案分页
        /// </summary>
        /// <param name="input">分页参数及筛选条件</param>
        /// <returns></returns>
        Task<IResponseOutput> PageAsync(PageInput<RecordEntity> input);

        /// <summary>
        /// 档案添加
        /// </summary>
        /// <param name="input">档案添加基本信息</param>
        /// <param name="fileInput">档案选中文件类别及其文件</param>
        /// <returns></returns>
        Task<IResponseOutput> AddAsync(RecordAddInput input, List<RecordFileTypeOutput> fileInput);

        /// <summary>
        /// 档案编辑
        /// </summary>
        /// <param name="input">档案编辑基本信息</param>
        /// <param name="fileInput">档案选中文件类别及其文件</param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAsync(RecordUpdateInput input, List<RecordFileTypeUpdateOutput> fileInput);

        /// <summary>
        /// 删除档案(包括所属文件)
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(long id);

        /// <summary>
        /// 获取档案的档案类别
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<long> GetRecordTypeAsync(long id);

        /// <summary>
        /// 获取档案信息
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetRecordInfoAsync(long id);

        /// <summary>
        /// 获取档案补充提交信息(包含已选中文件类别)
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetRecordAddtionalInfoAsync(long id);

        /// <summary>
        /// 获取档案(直接返回类)
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<RecordEntity> GetRecordAsync(long id);

        /// <summary>
        /// 档案补充提交添加
        /// </summary>
        /// <param name="record">档案基本信息</param>
        /// <param name="input">补充提交的档案类别</param>
        /// <returns></returns>
        Task<IResponseOutput> AddAdditionalRecordInfoAsync(RecordGetOutput record, List<RecordFileTypeAdditionalOutput> input);

        Task<IResponseOutput> HandOverPageAsync(PageInput<RecordEntity> input);

        /// <summary>
        /// 移交确认
        /// </summary>
        /// <param name="input">移交确认清单</param>
        /// <returns></returns>
        IResponseOutput HandOverCheckAsync(HandOverBasicInfoOutput input);

        Task<IResponseOutput> GetHandOverInfoAsync(long id);

        Task<IResponseOutput> GetListByUserAsync(long id);

        Task<IResponseOutput> RelationChangeAsync(List<RecordTransferInput> input);

        Task<IResponseOutput> GetPrintInfoAsync(long id);

        Task<IResponseOutput> GetExpiredRecordListAsync(PageInput<RecordEntity> input);

        Task<IResponseOutput> GetChangeDetailAsync(long id);

        Task<IResponseOutput> AppleChangeFileAsync(int type, List<RecordFileTypeOutput> input);

        Task<IResponseOutput> GetApplyChangeListAsync(PageInput<InitiativeUpdateEntity> input);

        Task<IResponseOutput> GetApplyChangeDetailAsync(long id);

        Task<IResponseOutput> AcceptApplyChangeAsync(long id);

        Task<IResponseOutput> RefuseApplyChangeAsync(long id, string refuseReason);

        Task<IResponseOutput> GetNeedCreateRecordList(int type, string userCode, long departmentCode, PageInput<NeedCreateRecordEntity> input);

        Task<IResponseOutput> StockAddAsync(RecordAddInput input, List<RecordFileTypeOutput> fileInput);

        Task<RecordEntity> GetRecordByIniId(long id);
    }
}
