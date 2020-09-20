using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Questionnaire;
using FreeSql;

namespace Admin.Core.Repository.Questionnaire.Appraise
{
    public class AppraiseRepository:RepositoryBase<AppraiseEntity>, IAppraiseRepository
    {
        public AppraiseRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
