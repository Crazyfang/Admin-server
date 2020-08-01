using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.RecordFile
{
    public class RecordFileRepository:RepositoryBase<RecordFileEntity>,IRecordFileRepository
    {
        public RecordFileRepository(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {
        }
    }
}
