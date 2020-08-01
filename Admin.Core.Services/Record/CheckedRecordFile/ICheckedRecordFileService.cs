using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.CheckedRecordFile.Input;

namespace Admin.Core.Service.Record.CheckedRecordFile
{
    public partial interface ICheckedRecordFileService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<List<CheckedRecordFileInput>> GetCheckedRecordFileAsync(long id, long recordId);
    }
}
