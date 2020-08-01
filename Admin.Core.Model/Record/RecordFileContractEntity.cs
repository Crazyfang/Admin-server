using System;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_recordfilecontract")]
    public class RecordFileContractEntity:EntityBase
    {
        public string ContractNo { get; set; }

        public int Num { get; set; }

        public long? RecordFileId { get; set; }
        public CheckedRecordFileEntity RecordFile { get; set; }

        public long? RecordOtherFileId { get; set; }
        public OtherRecordFileEntity RecordOtherFile { get; set; }
    }
}
