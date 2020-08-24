using System;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_initiativeupdateitem")]
    public class InitiativeUpdateItemEntity:EntityAdd
    {
        public long CheckedRecordFileId { get; set; }

        public CheckedRecordFileEntity CheckedRecordFile { get; set; }

        public bool DelSign { get; set; }

        public DateTime? CreditDueDate { get; set; }

        public long InitiativeUpdateId { get; set; }

        public InitiativeUpdateEntity InitiativeUpdate { get; set; }
    }
}
