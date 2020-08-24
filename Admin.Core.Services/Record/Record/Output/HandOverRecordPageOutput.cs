using System;
namespace Admin.Core.Service.Record.Record.Output
{
    public class HandOverRecordPageOutput
    {
        public long Id { get; set; }

        public string RecordId { get; set; }

        public int RecordType { get; set; }

        public string RecordUserName { get; set; }

        public string RecordUserCode { get; set; }

        public string RecordUserInCode { get; set; }

        public string RecordManagerName { get; set; }

        public string RecordManagerCode { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentCode { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
