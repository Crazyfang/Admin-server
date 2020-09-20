using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Questionnaire;
using FreeSql;

namespace Admin.Core.Repository.Questionnaire.HouseHoldMember
{
    public class HouseHoldMemberRepository:RepositoryBase<HouseHoldMemberEntity>, IHouseHoldMemberRepository
    {
        public HouseHoldMemberRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
