using System;
using System.Collections.Generic;
using Admin.Core.Service.Record.InitiativeUpdateItem.Output;

namespace Admin.Core.Service.Record.RecordFileType.Output
{
    public class ChangeFileRecordFileTypeOutput
    {
        public Guid Uid { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public long CheckedRecordFileTypeId { get; set; }

        public List<InitiativeUpdateItemOutput> Children { get; set; }

        public ChangeFileRecordFileTypeOutput()
        {
            Uid = Guid.NewGuid();
            Children = new List<InitiativeUpdateItemOutput>();
        }
    }
}
