using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Repository.Record.CheckedRecordFile;
using Admin.Core.Repository.Record.RecordFile;
using Admin.Core.Service.Record.CheckedRecordFile.Input;
using AutoMapper;

namespace Admin.Core.Service.Record.CheckedRecordFile
{
    public class CheckedRecordFileService:ICheckedRecordFileService
    {
        private readonly ICheckedRecordFileRepository _checkedRecordFileRepository;
        private readonly IRecordFileRepository _recordFileRepositroy;
        private readonly IMapper _mapper;
        public CheckedRecordFileService(ICheckedRecordFileRepository checkedRecordFileRepository, IMapper mapper, IRecordFileRepository recordFileRepository)
        {
            _checkedRecordFileRepository = checkedRecordFileRepository;
            _mapper = mapper;
            _recordFileRepositroy = recordFileRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var entity = await _checkedRecordFileRepository.GetAsync(id);

            return ResponseOutput.Ok(entity);
        }

        public async Task<List<CheckedRecordFileInput>> GetCheckedRecordFileAsync(long id, long recordId)
        {
            var entityList = await _checkedRecordFileRepository.Select
                .Where(i => i.RecordFile.RecordFileTypeId == id && i.RecordId == recordId)
                .ToListAsync();

            var recordFileList = await _recordFileRepositroy.Select
                .Where(i => i.RecordFileTypeId == id)
                .ToListAsync();

            recordFileList = recordFileList.Where(i => !entityList.Exists(m => m.RecordFileId == i.Id)).ToList();

            var output = _mapper.Map<List<CheckedRecordFileInput>>(entityList);
            var output1 = _mapper.Map<List<CheckedRecordFileInput>>(recordFileList);


            return output.Union(output1).OrderBy(i => i.Id).ToList();
        }
    }
}
