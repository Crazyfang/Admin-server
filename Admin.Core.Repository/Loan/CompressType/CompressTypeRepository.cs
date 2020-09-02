using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Loan;
using FreeSql;

namespace Admin.Core.Repository.Loan.CompressType
{
    public class CompressTypeRepository:RepositoryBase<CompressTypeEntity>, ICompressTypeRepository
    {
        public CompressTypeRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {

        }
    }
}
