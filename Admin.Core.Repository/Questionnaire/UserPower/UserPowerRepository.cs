using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Questionnaire;
using FreeSql;

namespace Admin.Core.Repository.Questionnaire.UserPower
{
    public class UserPowerRepository:RepositoryBase<UserPowerEntity>, IUserPowerRepository
    {
        public UserPowerRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
