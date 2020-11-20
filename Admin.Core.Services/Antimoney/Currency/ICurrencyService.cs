using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;

namespace Admin.Core.Service.Antimoney.Currency
{
    public partial interface ICurrencyService
    {
        /// <summary>
        /// 返回币种选择列表
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> ReturnSelectListAsync();
    }
}
