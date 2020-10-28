using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_recordfiletype")]
    public class RecordFileTypeEntity:EntityBase
    {
        /// <summary>
        /// 文件类型名称
        /// </summary>
        public string FileTypeName { get; set; }

        /// <summary>
        /// 档案类型
        /// </summary>
        public long? RecordTypeId { get; set; }
        public RecordTypeEntity RecordType { get; set; }

        /// <summary>
        /// 是否必须输入合同号
        /// </summary>
        public bool? HasContractNo { get; set; }

        [Navigate(nameof(RecordFileEntity.RecordFileTypeId))]
        public List<RecordFileEntity> RecordFileList { get; set; }

        public List<CheckedRecordFileEntity> CheckedRecordFileList { get; set; }
    }
}
