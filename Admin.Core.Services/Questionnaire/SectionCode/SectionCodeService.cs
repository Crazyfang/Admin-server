using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Repository.Questionnaire.SectionCode;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.SectionCode
{
    public class SectionCodeService:ISectionCodeService
    {
        private readonly ISectionCodeRepository _sectionCodeRepository;
        private readonly IMapper _mapper;

        public SectionCodeService(ISectionCodeRepository sectionCodeRepository
            , IMapper mapper)
        {
            _sectionCodeRepository = sectionCodeRepository;
            _mapper = mapper;
        }

        public async Task<IResponseOutput> GetListAsync()
        {
            var list = await _sectionCodeRepository.Select.ToListAsync();

            var data = _mapper.Map<List<SectionCodeListOutput>>(list);

            return ResponseOutput.Ok(data);
        }
    }
}
