using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_recordfile")]
    public class RecordFileEntity:EntityBase
    {
        /// <summary>
        /// 档案文件名称
        /// </summary>
        public string RecordFileName { get; set; }

        /// <summary>
        /// 档案文件类型
        /// </summary>
        public long? RecordFileTypeId { get; set; }
        public RecordFileTypeEntity RecordFileType { get; set; }

        //[Column(IsIgnore = true)]
        //public long? CheckedRecordFileId { get; set; }
    }
}
