using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.RecordFileContract
{
    public class RecordFileContractRepository:RepositoryBase<RecordFileContractEntity>, IRecordFileContractRepository
    {
        public RecordFileContractRepository(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {

        }
    }
}
