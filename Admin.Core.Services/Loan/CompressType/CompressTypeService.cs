using System;
using Admin.Core.Repository.Loan.CompressType;
using AutoMapper;

namespace Admin.Core.Service.Loan.CompressType
{
    public class CompressTypeService:ICompressTypeService
    {
        private readonly IMapper _mapper;
        private readonly ICompressTypeRepository _compressTypeRepository;

        public CompressTypeService(IMapper mapper
            , ICompressTypeRepository compressTypeRepository)
        {
            _mapper = mapper;
            _compressTypeRepository = compressTypeRepository;
        }
    }
}
