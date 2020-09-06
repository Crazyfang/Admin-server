using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Loan;
using FreeSql;

namespace Admin.Core.Repository.Loan.LoanMethod
{
    public class LoanMethodRepository:RepositoryBase<LoanMethodEntity>, ILoanMethodRepository
    {
        public LoanMethodRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
