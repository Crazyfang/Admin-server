using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_recordtype")]
    public class RecordTypeEntity:EntityBase
    {
        /// <summary>
        /// 档案类型名称
        /// </summary>
        public string RecordTypeName { get; set; }

        [Navigate(nameof(RecordFileTypeEntity.RecordTypeId))]
        public List<RecordFileTypeEntity> RecordFileTypeList { get; set; }
    }
}
