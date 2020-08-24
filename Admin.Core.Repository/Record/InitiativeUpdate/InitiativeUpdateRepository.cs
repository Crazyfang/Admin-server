using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.InitiativeUpdate
{
    public class InitiativeUpdateRepository:RepositoryBase<InitiativeUpdateEntity>, IInitiativeUpdateRepository
    {
        public InitiativeUpdateRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {

        }
    }
}
