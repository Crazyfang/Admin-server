using System;
using System.Collections.Generic;
using Admin.Core.Model.Record;

namespace Admin.Core.Service.Record.CheckedRecordFile.Input
{
    public class CheckedRecordFileInput
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long RecordFileId { get; set; }

        public long CheckedRecordFileId { get; set; }

        public DateTime? CreditDueDate { get; set; }

        public int Num { get; set; }

        public bool Checked { get; set; }
         
        public int OtherSign { get; set; }

        public int HandOverSign { get; set; }
    }
}
