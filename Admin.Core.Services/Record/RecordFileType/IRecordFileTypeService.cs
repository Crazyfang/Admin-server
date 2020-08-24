using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordFileType.Input;
using Admin.Core.Service.Record.RecordFileType.Output;

namespace Admin.Core.Service.Record.RecordFileType
{
    public partial interface IRecordFileTypeService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> GetRecordFileListAsync(long id);

        Task<IResponseOutput> AddRecordFileTypeAsync(RecordFileTypeAddInput input);

        Task<IResponseOutput> UpdateRecordFileTypeAsync(RecordFileTypeUpdateInput input);

        Task<IResponseOutput> DeleteRecordFileTypeAsync(long id);

        Task<List<RecordFileTypeUpdateOutput>> UpdateRecordPageListAsync(long id, long recordId);

        Task<List<RecordFileTypeAddOutput>> AddRecordPageListAsync(long id);
    }
}
