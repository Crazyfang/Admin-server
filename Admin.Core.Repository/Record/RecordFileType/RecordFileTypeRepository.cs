using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.RecordFileType
{
    public class RecordFileTypeRepository:RepositoryBase<RecordFileTypeEntity>, IRecordFileTypeRepository
    {
        public RecordFileTypeRepository(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {

        }
    }
}
