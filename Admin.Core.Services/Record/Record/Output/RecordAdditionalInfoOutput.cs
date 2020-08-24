using System;
using System.Collections.Generic;
using Admin.Core.Service.Record.RecordFileType.Output;

namespace Admin.Core.Service.Record.Record.Output
{
    public class RecordAdditionalInfoOutput
    {
        public RecordGetOutput Record { get; set; }

        public List<RecordFileTypeAdditionalOutput> RecordFileTypeList { get; set; }

        public RecordAdditionalInfoOutput()
        {
            RecordFileTypeList = new List<RecordFileTypeAdditionalOutput>();
        }
    }
}
