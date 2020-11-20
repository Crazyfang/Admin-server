using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Antimoney;
using FreeSql;

namespace Admin.Core.Repository.Antimoney.Company
{
    public class CompanyRepository:RepositoryBase<CompanyEntity>, ICompanyRepository
    {
        public CompanyRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {

        }
    }
}
