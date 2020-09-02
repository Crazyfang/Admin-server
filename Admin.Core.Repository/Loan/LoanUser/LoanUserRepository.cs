using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Loan;
using FreeSql;

namespace Admin.Core.Repository.Loan.LoanUser
{
    public class LoanUserRepository:RepositoryBase<LoanUserEntity>, ILoanUserRepository
    {
        public LoanUserRepository(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {

        }
    }
}
