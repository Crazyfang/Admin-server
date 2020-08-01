using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.OtherRecordFile
{
    public class OtherRecordFileRepositiry:RepositoryBase<OtherRecordFileEntity>, IOtherRecordFileRepositiry
    {
        public OtherRecordFileRepositiry(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {
        }
    }
}
