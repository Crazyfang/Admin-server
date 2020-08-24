using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.CheckedRecordFileType
{
    public class CheckedRecordFileTypeRepository:RepositoryBase<CheckedRecordFileTypeEntity>, ICheckedRecordFileTypeRepository
    {
        public CheckedRecordFileTypeRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {

        }
    }
}
