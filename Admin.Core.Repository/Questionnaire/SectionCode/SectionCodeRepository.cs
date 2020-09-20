using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Questionnaire;
using FreeSql;

namespace Admin.Core.Repository.Questionnaire.SectionCode
{
    public class SectionCodeRepository:RepositoryBase<SectionCodeEntity>, ISectionCodeRepository
    {
        public SectionCodeRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
