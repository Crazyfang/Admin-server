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
        Task<IResponseOutput> GetRecordAdditionalInfoAsync(long id);

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

        /// <summary>
        /// 移交档案清单分页
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<IResponseOutput> HandOverPageAsync(PageInput<RecordEntity> input);

        /// <summary>
        /// 移交确认
        /// </summary>
        /// <param name="input">移交确认清单</param>
        /// <returns></returns>
        IResponseOutput HandOverCheckAsync(HandOverBasicInfoOutput input);

        /// <summary>
        /// 获取移交基本信息
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetHandOverInfoAsync(long id);

        /// <summary>
        /// 根据用户获取所管理的档案
        /// </summary>
        /// <param name="id">用户主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetListByUserAsync(long id);

        /// <summary>
        /// 档案关系转让
        /// </summary>
        /// <param name="input">转让清单</param>
        /// <returns></returns>
        Task<IResponseOutput> RelationChangeAsync(List<RecordTransferInput> input);

        /// <summary>
        /// 获取档案移交清单打印信息
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetPrintInfoAsync(long id);

        /// <summary>
        /// 获取过期档案文件的档案分页
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<IResponseOutput> GetExpiredRecordListAsync(PageInput<RecordEntity> input);

        /// <summary>
        /// 获取申请档案文件变更信息
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetChangeDetailAsync(long id);

        /// <summary>
        /// 申请更换档案文件
        /// </summary>
        /// <param name="type">申请类型(0-过期,1-主动)</param>
        /// <param name="input">申请更换的档案文件</param>
        /// <returns></returns>
        Task<IResponseOutput> AppleChangeFileAsync(int type, List<RecordFileTypeOutput> input);

        /// <summary>
        /// 获取申请更换档案文件的列表
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<IResponseOutput> GetApplyChangeListAsync(PageInput<InitiativeUpdateEntity> input);

        /// <summary>
        /// 获取申请档案文件更换的具体细节
        /// </summary>
        /// <param name="id">申请主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetApplyChangeDetailAsync(long id);

        /// <summary>
        /// 同意档案文件更换申请
        /// </summary>
        /// <param name="id">申请主键</param>
        /// <returns></returns>
        Task<IResponseOutput> AcceptApplyChangeAsync(long id);

        /// <summary>
        /// 拒绝档案文件更换申请
        /// </summary>
        /// <param name="id">申请主键</param>
        /// <param name="refuseReason">拒绝原因</param>
        /// <returns></returns>
        Task<IResponseOutput> RefuseApplyChangeAsync(long id, string refuseReason);

        /// <summary>
        /// 获取名下待创建的档案清单
        /// </summary>
        /// <param name="type">人员类型</param>
        /// <param name="userCode">用户主键</param>
        /// <param name="departmentCode">部门主键</param>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<IResponseOutput> GetNeedCreateRecordList(int type, string userCode, long departmentCode, PageInput<NeedCreateRecordEntity> input);

        /// <summary>
        /// 存量档案添加
        /// </summary>
        /// <param name="input">档案添加基本信息</param>
        /// <param name="fileInput">档案文件</param>
        /// <returns></returns>
        Task<IResponseOutput> StockAddAsync(RecordAddInput input, List<RecordFileTypeOutput> fileInput);

        /// <summary>
        /// 根据申请主键获取更改的档案
        /// </summary>
        /// <param name="id">申请更换主键</param>
        /// <returns></returns>
        Task<RecordEntity> GetRecordByIniId(long id);

        /// <summary>
        /// 变更档案状态
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <param name="status">状态字</param>
        /// <returns></returns>
        Task<IResponseOutput> ChangeRecordStatusAsync(int status, long id);
    }
}
