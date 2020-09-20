using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Repository.Questionnaire.HouseHold;
using Admin.Core.Service.Questionnaire.HouseHold.Output;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.HouseHold
{
    public class HouseHoldService:IHouseHoldService
    {
        private readonly IHouseHoldRepository _houseHoldRepository;
        private readonly IMapper _mapper;

        public HouseHoldService(IMapper mapper
            , IHouseHoldRepository houseHoldRepository)
        {
            _mapper = mapper;
            _houseHoldRepository = houseHoldRepository;
        }

        public async Task<IResponseOutput> PageAsync(PageInput<HouseHoldEntity> input, List<string> belongedStreet, long userId)
        {
            var list = await _houseHoldRepository.Select
                .Where(i => !i.Appraises.AsSelect().Any(i => i.AppraiserId == userId))
                .Where(i => belongedStreet.Contains(i.BelongedStreet))
                .WhereIf(!string.IsNullOrEmpty(input.Filter.HeadUserName), i => i.HeadUserName.Contains(input.Filter.HeadUserName))
                .WhereIf(!string.IsNullOrEmpty(input.Filter.HeadUserIdNumber), i => i.HeadUserIdNumber.Contains(input.Filter.HeadUserIdNumber))
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var data = new PageOutput<HouseHoldPageOutput>()
            {
                List = _mapper.Map<List<HouseHoldPageOutput>>(list),
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> CalculatePageAsync(string code)
        {
            var list = await _houseHoldRepository.Select
                .Include(i => i.SuggestCreditor)
                .Where(i => i.BelongedStreet == code)
                .ToListAsync();

            var output = _mapper.Map<List<HouseHoldCalculateOutput>>(list);

            return ResponseOutput.Ok(output);
        }
    }
}
