using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Loan.LoanUser.Input;

namespace Admin.Core.Service.Loan.LoanUser
{
    public partial interface ILoanUserService
    {
        Task<IResponseOutput> AddLoanUserAsync(LoanUserAddInput input);
    }
}
