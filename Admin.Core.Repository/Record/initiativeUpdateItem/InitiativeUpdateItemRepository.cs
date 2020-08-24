using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.initiativeUpdateItem
{
    public class InitiativeUpdateItemRepository:RepositoryBase<InitiativeUpdateItemEntity>, IInitiativeUpdateItemRepository
    {
        public InitiativeUpdateItemRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {

        }
    }
}
