using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Repository.Record.RecordFile;
using Admin.Core.Service.Record.RecordFile.Input;
using Admin.Core.Service.Record.RecordFile.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordFile
{
    public class RecordFileService:IRecordFileService
    {
        private readonly IMapper _mapper;
        private IRecordFileRepository _recordFileRepository;

        public RecordFileService(IMapper mapper, IRecordFileRepository recordFileRepository)
        {
            _mapper = mapper;
            _recordFileRepository = recordFileRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var entity = await _recordFileRepository.GetAsync(id);

            return ResponseOutput.Ok(entity);
        }

        public async Task<IResponseOutput> GetRecordFileListAsync(long id)
        {
            var enetity = await _recordFileRepository.Select
                .Where(i => i.RecordFileTypeId == id)
                .ToListAsync();

            var output = _mapper.Map<List<RecordFileListOutput>>(enetity);

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> AddAsync(RecordFileAddInput input)
        {
            var entity = _mapper.Map<RecordFileEntity>(input);

            var addSign = (await _recordFileRepository.InsertAsync(entity)).Id > 0;
            return ResponseOutput.Result(addSign);
        }

        public async Task<IResponseOutput> UpdateAsync(RecordFileUpdateInput input)
        {
            var entity = await _recordFileRepository.GetAsync(input.Id);

            if(!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("当前档案文件未找到");
            }
            else
            {
                _mapper.Map(input, entity);
                await _recordFileRepository.UpdateAsync(entity);

                return ResponseOutput.Ok();
            }
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _recordFileRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<List<NewRecordFileOutput>> GetRecordFileByRecordTypeIdAsync(long id)
        {
            var entityList = await _recordFileRepository.Select
                .Where(i => i.RecordFileTypeId == id)
                .ToListAsync();

            var output = _mapper.Map<List<NewRecordFileOutput>>(entityList);

            return output;
        }
    }
}
