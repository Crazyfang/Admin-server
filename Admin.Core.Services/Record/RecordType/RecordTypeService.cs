using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Repository.Record.RecordType;
using Admin.Core.Service.Record.RecordType.Input;
using Admin.Core.Service.Record.RecordType.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordType
{
    public class RecordTypeService:IRecordTypeService
    {
        private readonly IMapper _mapper;
        private IRecordTypeRepository _recordTypeRepository;

        public RecordTypeService(IMapper mapper, IRecordTypeRepository recordTypeRepository)
        {
            _mapper = mapper;
            _recordTypeRepository = recordTypeRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var entity = await _recordTypeRepository.GetAsync(id);

            return ResponseOutput.Ok(entity);
        }

        public async Task<IResponseOutput> GetAllRecordTypeAsync()
        {
            var entityList = await _recordTypeRepository.Select.ToListAsync();

            var returnList = _mapper.Map<List<RecordTypeListOutput>>(entityList);
            return ResponseOutput.Ok(returnList);
        }

        public async Task<IResponseOutput> AddRecordTypeAsync(RecordTypeAddInput input)
        {
            var entity = _mapper.Map<RecordTypeEntity>(input);
            var recordType = await _recordTypeRepository.InsertAsync(entity);

            return recordType.Id > 0 ? ResponseOutput.Ok() : ResponseOutput.NotOk("增加档案类型失败!");
        }

        public async Task<IResponseOutput> UpdateRecordTypeAsync(RecordTypeUpdateInput input)
        {
            var entity = await _recordTypeRepository.GetAsync(input.Id);

            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("当前档案类型不存在，无法进行更新!");
            }
            else
            {
                _mapper.Map(input, entity);
                await _recordTypeRepository.UpdateAsync(entity);

                return ResponseOutput.Ok();
            }
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _recordTypeRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _recordTypeRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }
    }
}
