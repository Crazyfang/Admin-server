using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.CheckedRecordFile
{
    public class CheckedRecordFileRepository:RepositoryBase<CheckedRecordFileEntity>, ICheckedRecordFileRepository
    {
        public CheckedRecordFileRepository(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {

        }
    }
}
