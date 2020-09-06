using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.LoanUser.Input;

namespace Admin.Core.Service.Loan.LoanUser
{
    public partial interface ILoanUserService
    {
        /// <summary>
        /// 添加贷款压缩信息
        /// </summary>
        /// <param name="input">贷款压缩添加信息</param>
        /// <returns></returns>
        Task<IResponseOutput> AddLoanUserAsync(LoanUserAddInput input);

        /// <summary>
        /// 编辑贷款压缩信息
        /// </summary>
        /// <param name="input">贷款压缩编辑信息</param>
        /// <returns></returns>
        Task<IResponseOutput> EditLoanUserAsync(LoanUserEditInput input);

        /// <summary>
        /// 获取贷款压缩基础信息
        /// </summary>
        /// <param name="id">贷款压缩主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetLoanUserInfoAsync(long id);

        /// <summary>
        /// 获取贷款压缩分页
        /// </summary>
        /// <param name="input">筛选条件，分页信息</param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageAsync(PageInput<LoanUserEntity> input);

        /// <summary>
        /// 删除贷款压缩信息，包括条目
        /// </summary>
        /// <param name="id">贷款压缩主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteLoanUserAsync(long id);

        /// <summary>
        /// 审核通过贷款压缩信息
        /// </summary>
        /// <param name="id">贷款压缩主键</param>
        /// <returns></returns>
        Task<IResponseOutput> VerifyLoanUserAsync(long id, long userId);
    }
}
