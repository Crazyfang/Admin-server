using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.Record
{
    public class RecordRepository:RepositoryBase<RecordEntity>, IRecordRepository
    {
        public RecordRepository(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {

        }
    }
}
