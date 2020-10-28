using System;
using System.Collections.Generic;

namespace Admin.Core.Service.Record.RecordFile.Output
{
    public class AddRecordFileOutput
    {
        public Guid Uid { get; set; }

        public long RecordFileId { get; set; }

        public long CheckedRecordFleId { get; set; }

        public string Name { get; set; }

        public DateTime? CreditDueDate { get; set; }

        public int Num { get; set; }

        public bool Checked { get; set; }

        public int OtherSign { get; set; }

        public int HandOverSign { get; set; }

        public bool? HasCreditDueDate { get; set; }

        public AddRecordFileOutput()
        {
            Uid = Guid.NewGuid();
            Num = 1;
        }
    }
}
