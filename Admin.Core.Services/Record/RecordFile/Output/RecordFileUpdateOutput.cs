using System;
namespace Admin.Core.Service.Record.RecordFile.Output
{
    public class RecordFileUpdateOutput
    {
        public Guid Uid { get; set; }

        public long RecordFileId { get; set; }

        public long CheckedRecordFileId { get; set; }

        public string Name { get; set; }

        public DateTime? CreditDueDate { get; set; }

        public int Num { get; set; }

        public bool Checked { get; set; }

        public int OtherSign { get; set; }

        public int HandOverSign { get; set; }

        public RecordFileUpdateOutput()
        {
            Uid = Guid.NewGuid();
        }
    }
}
