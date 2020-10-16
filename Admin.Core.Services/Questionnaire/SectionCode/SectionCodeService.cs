using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Repository.Questionnaire.SectionCode;
using Admin.Core.Repository.Questionnaire.UserPower;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.SectionCode
{
    public class SectionCodeService:ISectionCodeService
    {
        private readonly ISectionCodeRepository _sectionCodeRepository;
        private readonly IUserPowerRepository _userPowerRepository;
        private readonly IMapper _mapper;

        public SectionCodeService(ISectionCodeRepository sectionCodeRepository
            , IUserPowerRepository userPowerRepository
            , IMapper mapper)
        {
            _sectionCodeRepository = sectionCodeRepository;
            _userPowerRepository = userPowerRepository;
            _mapper = mapper;
        }

        public async Task<IResponseOutput> GetListAsync(long userId)
        {
            var sectionListStr = await _userPowerRepository.Select
                .Where(i => i.UserId == userId)
                .ToOneAsync(i => i.Power);
            var sectionList = new List<string>(sectionListStr.Split(","));
            var list = await _sectionCodeRepository.Select
                .Where(i => sectionList.Contains(i.VillageCode))
                .ToListAsync();

            var data = _mapper.Map<List<SectionCodeListOutput>>(list);

            return ResponseOutput.Ok(data);
        }
    }
}
