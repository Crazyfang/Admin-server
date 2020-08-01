using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Record.OtherRecordFile.Output;

namespace Admin.Core.Service.Record.OtherRecordFile
{
    public partial interface IOtherRecordFileService
    {
        Task<List<OtherRecordFileOutput>> GetOtherRecordFileByRecordFileTypeId(long id);
    }
}
