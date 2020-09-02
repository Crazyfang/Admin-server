using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    [Table(Name = "rt_record_history")]
    public class RecordHistoryEntity:EntityAdd
    {
        [Description("档案主键")]
        public long RecordId { get; set; }

        [Description("操作类型")]
        public string OperateType { get; set; }

        [Description("操作内容")]
        [MaxLength(-1)]
        public string OperateInfo { get; set; }

        public UserEntity CreatedUser { get; set; }
    }
}
