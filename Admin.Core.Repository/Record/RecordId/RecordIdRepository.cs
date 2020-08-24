using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.RecordId
{
    public class RecordIdRepository:RepositoryBase<RecordIdEntity>, IRecordIdRepository
    {
        public RecordIdRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {

        }
    }
}
