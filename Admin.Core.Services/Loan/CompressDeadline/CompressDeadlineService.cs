using System;
using Admin.Core.Repository.Loan.CompressDeadline;
using AutoMapper;

namespace Admin.Core.Service.Loan.CompressDeadline
{
    public class CompressDeadlineService:ICompressDeadlineService
    {
        private readonly IMapper _mapper;
        private readonly ICompressDeadlineRepository _compressDeadlineRepository;

        public CompressDeadlineService(IMapper mapper
            , ICompressDeadlineRepository compressDeadlineRepository)
        {
            _mapper = mapper;
            _compressDeadlineRepository = compressDeadlineRepository;
        }
    }
}
