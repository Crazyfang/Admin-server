using System;
using System.Collections.Generic;
using Admin.Core.Service.Record.CheckedRecordFile.Input;

namespace Admin.Core.Service.Record.RecordFileType.Output
{
    public class RecordFileTypeOutput
    {
        public Guid Uid { get; set; }

        public long RecordFileTypeId { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public long CheckedRecordFileTypeId { get; set; }

        public List<CheckedRecordFileInput> Children { get; set; }

        public RecordFileTypeOutput()
        {
            Uid = Guid.NewGuid();
            Children = new List<CheckedRecordFileInput>();
        }
    }
}
