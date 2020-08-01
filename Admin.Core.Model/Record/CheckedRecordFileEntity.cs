using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_checkedrecordfile")]
    public class CheckedRecordFileEntity: EntityBase
    {
        public long? RecordId { get; set; }
        public RecordEntity Record { get; set; }

        public long RecordFileId { get; set; }
        public RecordFileEntity RecordFile { get; set; }

        public DateTime? CreditDueDate { get; set; }

        public int Num { get; set; }

        [Navigate(nameof(RecordFileContractEntity.RecordFileId))]
        public List<RecordFileContractEntity> Contract { get; set; }
    }
}
