using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Loan;

namespace Admin.Core.Service.Loan.LoanMethod
{
    public partial interface ILoanMethodService
    {
        /// <summary>
        /// 添加贷款方式变更
        /// </summary>
        /// <param name="input">贷款方式变更类</param>
        /// <returns></returns>
        Task<IResponseOutput> AddLoanMethodAsync(LoanMethodEntity input);

        /// <summary>
        /// 编辑贷款方式变更
        /// </summary>
        /// <param name="input">贷款方式变更类</param>
        /// <returns></returns>
        Task<IResponseOutput> EditLoanMethodAsync(LoanMethodEntity input);

        /// <summary>
        /// 删除贷款方式变更
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DelLoanMethodAsync(long id);

        /// <summary>
        /// 获取贷款方式变更
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetLoanMethodAsync(long id);

        /// <summary>
        /// 获取贷款方式变更分页
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<IResponseOutput> GetLoanMethodPageAsync(PageInput<LoanMethodEntity> input);

        /// <summary>
        /// 审核贷款方式变更
        /// </summary>
        /// <param name="id">贷款方式主键</param>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        Task<IResponseOutput> VerifyLoanMethodAsync(long id, long userId);
    }
}
