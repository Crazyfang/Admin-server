using System;
using Admin.Core.Repository.Record.CheckedRecordFileType;
using AutoMapper;

namespace Admin.Core.Service.Record.CheckedRecordFileType
{
    public class CheckedRecordFileTypeService:ICheckedRecordFileTypeService
    {
        private readonly IMapper _mapper;
        private readonly ICheckedRecordFileTypeRepository _checkedRecordFileTypeRepository;
        public CheckedRecordFileTypeService(IMapper mapper, ICheckedRecordFileTypeRepository checkedRecordFileTypeRepository)
        {
            _mapper = mapper;
            _checkedRecordFileTypeRepository = checkedRecordFileTypeRepository;
        }
    }
}
