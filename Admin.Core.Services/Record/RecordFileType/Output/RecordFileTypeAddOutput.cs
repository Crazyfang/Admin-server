using System;
using System.Collections.Generic;
using Admin.Core.Service.Record.RecordFile.Output;

namespace Admin.Core.Service.Record.RecordFileType.Output
{
    public class RecordFileTypeAddOutput
    {
        public Guid Uid { get; set; }

        public long RecordFileTypeId { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public List<AddRecordFileOutput> Children { get; set; }

        public RecordFileTypeAddOutput()
        {
            Uid = Guid.NewGuid();
        }
    }
}
