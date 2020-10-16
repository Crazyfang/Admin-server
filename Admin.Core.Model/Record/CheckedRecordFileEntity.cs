using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_checkedrecordfile")]
    public class CheckedRecordFileEntity: EntityBase
    {
        /// <summary>
        /// 档案主键
        /// </summary>
        public long? RecordId { get; set; }
        public RecordEntity Record { get; set; }

        /// <summary>
        /// 关联文件主键
        /// 0-代表是自定义文件
        /// </summary>
        public long RecordFileId { get; set; }
        public RecordFileEntity RecordFile { get; set; }

        /// <summary>
        /// 选中文件类型主键
        /// </summary>
        public long CheckedRecordFileTypeId { get; set; }
        public CheckedRecordFileTypeEntity CheckedRecordFileType { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? CreditDueDate { get; set; }

        /// <summary>
        /// 份数
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 自定义文件标识
        /// 0-预设文件
        /// 1-自定义文件
        /// </summary>
        public int OtherSign { get; set; }

        /// <summary>
        /// 移交标识
        /// 0-未移交
        /// 1-移交
        /// 2-待更改
        /// </summary>
        public int HandOverSign { get; set; }
    }
}
