using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.RecordBorroItem
{
    public class RecordBorrowItemRepository:RepositoryBase<RecordBorrowItemEntity>, IRecordBorrowItemRepository
    {
        public RecordBorrowItemRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
