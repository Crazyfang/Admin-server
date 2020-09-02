using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Loan;
using FreeSql;

namespace Admin.Core.Repository.Loan.CompressDeadline
{
    public class CompressDeadlineRepository:RepositoryBase<CompressDeadlineEntity>, ICompressDeadlineRepository
    {
        public CompressDeadlineRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {

        }
    }
}
