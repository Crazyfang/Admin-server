using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Admin;
using FreeSql;

namespace Admin.Core.Repository.Admin.RecordHistory
{
    public class RecordHistoryRepository:RepositoryBase<RecordHistoryEntity>, IRecordHistoryRepository
    {
        public RecordHistoryRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {

        }
    }
}
