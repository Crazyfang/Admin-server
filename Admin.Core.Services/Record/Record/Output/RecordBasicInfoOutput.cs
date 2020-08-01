using System;
using System.Collections.Generic;
using Admin.Core.Service.Record.RecordFileType.Output;

namespace Admin.Core.Service.Record.Record.Output
{
    public class RecordBasicInfoOutput
    {
        public RecordGetOutput Record { get; set; }

        public List<RecordFileTypeOutput> RecordFileTypeList { get; set; }

        public RecordBasicInfoOutput()
        {
            RecordFileTypeList = new List<RecordFileTypeOutput>();
        }
    }
}
