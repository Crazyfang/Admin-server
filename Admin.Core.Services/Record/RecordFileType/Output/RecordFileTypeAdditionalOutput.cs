using System;
using System.Collections.Generic;
using Admin.Core.Service.Record.RecordFile.Output;

namespace Admin.Core.Service.Record.RecordFileType.Output
{
    public class RecordFileTypeAdditionalOutput
    {
        public Guid Uid { get; set; }

        public long RecordFileTypeId { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public long CheckedRecordFileTypeId { get; set; }

        public List<RecordFileAdditionalOuput> Children { get; set; }

        public RecordFileTypeAdditionalOutput()
        {
            Uid = Guid.NewGuid();
            Children = new List<RecordFileAdditionalOuput>();
        }
    }
}
