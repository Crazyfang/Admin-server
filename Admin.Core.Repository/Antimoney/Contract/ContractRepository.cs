using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Antimoney;
using FreeSql;

namespace Admin.Core.Repository.Antimoney.Contract
{
    public class ContractRepository:RepositoryBase<ContractEntity>, IContractRepository
    {
        public ContractRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
