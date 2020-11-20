using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Antimoney;
using FreeSql;

namespace Admin.Core.Repository.Antimoney.File
{
    public class FileRepository:RepositoryBase<FileEntity>, IFileRepository
    {
        public FileRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
