using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.RecordBorrow
{
    public class RecordBorrowRepository:RepositoryBase<RecordBorrowEntity>, IRecordBorrowRepository
    {
        public RecordBorrowRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
