using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.RecordType
{
    public class RecordTypeRepository:RepositoryBase<RecordTypeEntity>,IRecordTypeRepository
    {
        public RecordTypeRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
