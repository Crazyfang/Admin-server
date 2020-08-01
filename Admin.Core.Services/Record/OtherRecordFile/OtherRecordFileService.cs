using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Repository.Record.OtherRecordFile;
using Admin.Core.Service.Record.OtherRecordFile.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.OtherRecordFile
{
    public class OtherRecordFileService:IOtherRecordFileService
    {
        private readonly IMapper _mapper;
        private readonly IOtherRecordFileRepositiry _otherRecordFileRepository;
        public OtherRecordFileService(IMapper mapper, IOtherRecordFileRepositiry otherRecordFileRepository)
        {
            _mapper = mapper;
            _otherRecordFileRepository = otherRecordFileRepository;
        }

        public async Task<List<OtherRecordFileOutput>> GetOtherRecordFileByRecordFileTypeId(long id)
        {
            var otherFileList = await _otherRecordFileRepository.Select
                .Where(i => i.RecordFileTypeId == id)
                .ToListAsync();

            var outputList = _mapper.Map<List<OtherRecordFileOutput>>(otherFileList);

            return outputList;
        }
    }
}
