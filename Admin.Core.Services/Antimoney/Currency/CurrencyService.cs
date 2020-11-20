using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Repository.Antimoney.Currency;
using Admin.Core.Service.Antimoney.Currency.Output;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.Currency
{
    public class CurrencyService:ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        public CurrencyService(ICurrencyRepository currencyRepository
            , IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<IResponseOutput> ReturnSelectListAsync()
        {
            var list = await _currencyRepository.Select
                .ToListAsync();

            var output = _mapper.Map<List<CurrencyListOutput>>(list);

            return ResponseOutput.Ok(output);
        }
    }
}
