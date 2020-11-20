using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.Contract.Input;

namespace Admin.Core.Service.Antimoney.Contract
{
    public partial interface IContractService
    {
        /// <summary>
        /// 合同分页
        /// </summary>
        /// <param name="input">分页信息</param>
        /// <returns></returns>
        Task<IResponseOutput> ContractPageAsync(PageInput<ContractSearchInput> input);

        /// <summary>
        /// 添加合同
        /// </summary>
        /// <param name="input">合同基本信息</param>
        /// <returns></returns>
        Task<IResponseOutput> AddContractAsync(ContractAddInput input);

        /// <summary>
        /// 编辑合同
        /// </summary>
        /// <param name="input">合同基本信息</param>
        /// <returns></returns>
        Task<IResponseOutput> EditContractAsync(ContractEditInput input);

        /// <summary>
        /// 根据合同号返回文件列表
        /// </summary>
        /// <param name="contractId">合同号</param>
        /// <returns></returns>
        Task<IResponseOutput> ReturnFileByContractIdAsync(long contractId);

        /// <summary>
        /// 根据主键获取合同信息
        /// </summary>
        /// <param name="contractId">合同主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetContractAsync(long contractId);

        /// <summary>
        /// 根据主键删除合同信息
        /// </summary>
        /// <param name="contratId">合同主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteContractAsync(long contratId);

        /// <summary>
        /// 获取通知
        /// </summary>
        /// <param name="contractId">合同主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetNoticeAsync(long contractId);

        /// <summary>
        /// 添加或更新通知
        /// </summary>
        /// <param name="input">添加内容</param>
        /// <returns></returns>
        Task<IResponseOutput> AddOrEditNoticeAsync(ContractNoticeInput input);

        /// <summary>
        /// 生成合同Excel表
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<IResponseOutput> GenerateContractListAsync(PageInput<ContractSearchInput> input);
    }
}
