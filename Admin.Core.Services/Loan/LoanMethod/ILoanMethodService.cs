using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Loan;

namespace Admin.Core.Service.Loan.LoanMethod
{
    public partial interface ILoanMethodService
    {
        Task<IResponseOutput> AddLoanMethodAsync(LoanMethodEntity input);

        Task<IResponseOutput> EditLoanMethodAsync(LoanMethodEntity input);

        Task<IResponseOutput> DelLoanMethodAsync(long id);

        Task<IResponseOutput> GetLoanMethodAsync(long id);

        Task<IResponseOutput> GetLoanMethodPageAsync(PageInput<LoanMethodEntity> input);

        Task<IResponseOutput> VerifyLoanMethodAsync(long id, long userId);
    }
}
