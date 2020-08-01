using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.RecordFile.Input;
using Admin.Core.Service.Record.RecordFile.Output;

namespace Admin.Core.Service.Record.RecordFile
{
    public partial interface IRecordFileService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> GetRecordFileListAsync(long id);

        Task<IResponseOutput> AddAsync(RecordFileAddInput input);

        Task<IResponseOutput> UpdateAsync(RecordFileUpdateInput input);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<List<NewRecordFileOutput>> GetRecordFileByRecordTypeIdAsync(long id);
    }
}
