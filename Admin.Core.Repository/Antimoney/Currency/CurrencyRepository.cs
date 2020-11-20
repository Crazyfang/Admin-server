using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Antimoney;
using FreeSql;

namespace Admin.Core.Repository.Antimoney.Currency
{
    public class CurrencyRepository:RepositoryBase<CurrencyEntity>, ICurrencyRepository
    {
        public CurrencyRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
