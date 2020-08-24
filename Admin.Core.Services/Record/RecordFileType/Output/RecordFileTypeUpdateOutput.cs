using System;
using System.Collections.Generic;
using Admin.Core.Service.Record.RecordFile.Output;

namespace Admin.Core.Service.Record.RecordFileType.Output
{
    public class RecordFileTypeUpdateOutput
    {
        public Guid Uid { get; set; }

        public long RecordFileTypeId { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public long CheckedRecordFileTypeId { get; set; }

        public List<RecordFileUpdateOutput> Children { get; set; }

        public RecordFileTypeUpdateOutput()
        {
            Uid = Guid.NewGuid();
        }
    }
}
