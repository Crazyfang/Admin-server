using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Questionnaire;
using FreeSql;

namespace Admin.Core.Repository.Questionnaire.HouseHold
{
    public class HouseHoldRepository:RepositoryBase<HouseHoldEntity>, IHouseHoldRepository
    {
        public HouseHoldRepository(UnitOfWorkManager uowm, IUser user):base(uowm, user)
        {

        }
    }
}
