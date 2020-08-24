using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Repository.Record.RecordHistory;
using Admin.Core.Service.Record.RecordHistory.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordHistory
{
    public class RecordHistoryService:IRecordHistoryService
    {
        private readonly IRecordHistoryRepository _recordHistoryRepository;
        private readonly IMapper _mapper;

        public RecordHistoryService(IMapper mapper, IRecordHistoryRepository recordHistoryRepository)
        {
            _mapper = mapper;
            _recordHistoryRepository = recordHistoryRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var recordHistoryList = await _recordHistoryRepository.Select.Where(i => i.RecordId == id).Include(i => i.CreatedUser).OrderByDescending(i => i.CreatedTime).ToListAsync();
            var output = _mapper.Map<List<RecordHistoryGetOutput>>(recordHistoryList);

            return ResponseOutput.Ok(output);
        }
    }
}
