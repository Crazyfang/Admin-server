using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Record;
using FreeSql;

namespace Admin.Core.Repository.Record.Notify
{
    public class NotifyRepository:RepositoryBase<NotifyEntity>, INotifyRepository
    {
        public NotifyRepository(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {

        }
    }
}
