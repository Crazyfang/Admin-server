using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Antimoney;
using FreeSql;

namespace Admin.Core.Repository.Antimoney.Picture
{
    public class PictureRepository:RepositoryBase<PictureEntity>, IPictureRepository
    {
        public PictureRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
