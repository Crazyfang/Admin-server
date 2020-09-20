using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Questionnaire;
using FreeSql;

namespace Admin.Core.Repository.Questionnaire.Calculate
{
    public class CalculateRepository:RepositoryBase<CalculateEntity>, ICalculateRepository
    {
        public CalculateRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
