using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Antimoney.Currency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Antimoney
{
    public class CurrencyController:AreaController
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// 获取币种Select数据
        /// </summary>
        /// <returns>币种Select</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> List()
        {
            return await _currencyService.ReturnSelectListAsync();
        }
    }
}
