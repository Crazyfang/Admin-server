using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_otherrecordfile")]
    public class OtherRecordFileEntity:EntityBase
    {
        public long? RecordId { get; set; }
        public RecordEntity Record { get; set; }

        public long RecordFileTypeId { get; set; }
        public RecordFileTypeEntity RecordFileType { get; set; }

        public string OtherFileName { get; set; }

        public int Num { get; set; }

        public DateTime? CreditDueDate { get; set; }

        [Navigate(nameof(RecordFileContractEntity.RecordOtherFileId))]
        public List<RecordFileContractEntity> Contract { get; set; }
    }
}
