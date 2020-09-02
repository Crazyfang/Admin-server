using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.CheckedRecordFile.Input;
using Admin.Core.Service.Record.Record.Input;
using Admin.Core.Service.Record.Record.Output;
using Admin.Core.Service.Record.RecordFileType.Output;

namespace Admin.Core.Service.Record.Record
{
    public partial interface IRecordService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> PageAsync(PageInput<RecordEntity> input);

        Task<IResponseOutput> AddAsync(RecordAddInput input, List<RecordFileTypeOutput> fileInput);

        Task<IResponseOutput> UpdateAsync(RecordUpdateInput input, List<RecordFileTypeUpdateOutput> fileInput);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<long> GetRecordTypeAsync(long id);

        Task<IResponseOutput> GetRecordInfoAsync(long id);

        Task<IResponseOutput> GetRecordAddtionalInfoAsync(long id);

        Task<RecordEntity> GetRecordAsync(long id);

        Task<IResponseOutput> AddAdditionalRecordInfoAsync(RecordGetOutput record, List<RecordFileTypeAdditionalOutput> input);

        Task<IResponseOutput> HandOverPageAsync(PageInput<RecordEntity> input);

        Task<IResponseOutput> HandOverCheckAsync(HandOverBasicInfoOutput input);

        Task<IResponseOutput> GetHandOverInfoAsync(long id);

        Task<IResponseOutput> GetListByUserAsync(long id);

        Task<IResponseOutput> RelationChangeAsync(List<RecordTransferInput> input);

        Task<IResponseOutput> GetPrintInfoAsync(long id);

        Task<IResponseOutput> GetExpiredRecordListAsync(PageInput<RecordEntity> input);

        Task<IResponseOutput> GetChangeDetailAsync(long id);

        Task<IResponseOutput> AppleChangeFileAsync(int type, List<RecordFileTypeOutput> input);

        Task<IResponseOutput> GetApplyChangeListAsync(PageInput<InitiativeUpdateEntity> input);

        Task<IResponseOutput> GetApplyChangeDetailAsync(long id);

        Task<IResponseOutput> AcceptApplyChangeAsync(long id);

        Task<IResponseOutput> RefuseApplyChangeAsync(long id, string refuseReason);

        Task<IResponseOutput> GetNeedCreateRecordList(int type, string userCode, long departmentCode, PageInput<NeedCreateRecordEntity> input);

        Task<IResponseOutput> StockAddAsync(RecordAddInput input, List<RecordFileTypeOutput> fileInput);

        Task<RecordEntity> GetRecordByIniId(long id);
    }
}
