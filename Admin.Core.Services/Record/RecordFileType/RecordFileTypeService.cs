using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Repository.Record.CheckedRecordFile;
using Admin.Core.Repository.Record.RecordFileType;
using Admin.Core.Service.Record.RecordFileType.Input;
using Admin.Core.Service.Record.RecordFileType.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordFileType
{
    public class RecordFileTypeService:IRecordFileTypeService
    {
        private readonly IMapper _mapper;
        private readonly IRecordFileTypeRepository _recordFileTypeRepository;
        public RecordFileTypeService(IMapper mapper, IRecordFileTypeRepository recordFileTypeRepository)
        {
            _mapper = mapper;
            _recordFileTypeRepository = recordFileTypeRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var entity = await _recordFileTypeRepository.GetAsync(id);

            return ResponseOutput.Ok(entity);
        }

        public async Task<IResponseOutput> GetRecordFileListAsync(long id)
        {
            var entityList = await _recordFileTypeRepository.Select
                .Where(i => i.RecordTypeId == id)
                .ToListAsync();

            var output = _mapper.Map<List<RecordFileTypeListOutput>>(entityList);

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> AddRecordFileTypeAsync(RecordFileTypeAddInput input)
        {
            var entity = _mapper.Map<RecordFileTypeEntity>(input);

            var addSign = (await _recordFileTypeRepository.InsertAsync(entity)).Id > 0;
            return ResponseOutput.Result(addSign);
        }

        public async Task<IResponseOutput> UpdateRecordFileTypeAsync(RecordFileTypeUpdateInput input)
        {
            var entity = await _recordFileTypeRepository.GetAsync(input.Id);

            if(!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("档案更新的档案文件类型不存在");
            }
            else
            {
                _mapper.Map(input, entity);
                await _recordFileTypeRepository.UpdateAsync(entity);

                return ResponseOutput.Ok();
            }
        }

        public async Task<IResponseOutput> DeleteRecordFileTypeAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _recordFileTypeRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }


        public async Task<List<RecordFileTypeOutput>> UpdateRecordPageListAsync(long id, long recordId)
        {
            var entityList = await _recordFileTypeRepository.Select
                .Where(i => i.RecordTypeId == id)
                .IncludeMany(i => i.OtherRecordFileList, then=>then.Where(i => i.RecordId == recordId).IncludeMany(m => m.Contract))
                .ToListAsync();

            var output = _mapper.Map<List<RecordFileTypeOutput>>(entityList);

            return output;
        }

        public async Task<List<RecordFileTypeAddOutput>> AddRecordPageListAsync(long id)
        {
            var entityList = await _recordFileTypeRepository.Select
                .Where(i => i.RecordTypeId == id)
                .IncludeMany(i => i.RecordFileList)
                .ToListAsync();

            var output = _mapper.Map<List<RecordFileTypeAddOutput>>(entityList);

            return output;
        }
    }
}
