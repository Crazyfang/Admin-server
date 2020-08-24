using System;
using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_checkedrecordfiletype")]
    public class CheckedRecordFileTypeEntity
    {
        [Column(IsIdentity =true, IsPrimary =true)]
        public long Id { get; set; }

        /// <summary>
        /// 关联文件类型主键
        /// </summary>
        public long RecordFileTypeId { get; set; }

        /// <summary>
        /// 档案主键
        /// </summary>
        public long RecordId { get; set; }

        public RecordEntity Record { get; set; }

        public RecordFileTypeEntity RecordFileType { get; set; }

        /// <summary>
        /// 备注项（合同号或者担保人名称）
        /// </summary>
        public string Remarks { get; set; }

        [Navigate(nameof(CheckedRecordFileEntity.CheckedRecordFileTypeId))]
        public List<CheckedRecordFileEntity> CheckedRecordFileList { get; set; }
    }
}
