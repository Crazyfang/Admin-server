using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Questionnaire;
using FreeSql;

namespace Admin.Core.Repository.Questionnaire.MemberResidence
{
    public class MemberResidenceRepository:RepositoryBase<MemberResidenceEntity>, IMemberResidenceRepository
    {
        public MemberResidenceRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
