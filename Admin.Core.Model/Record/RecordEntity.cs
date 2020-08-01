using System;
using Admin.Core.Common.BaseModel;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_record")]
    public class RecordEntity : EntityBase
    {
        /// <summary>
        /// 档案编号
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// 档案用户姓名
        /// </summary>
        public string RecordUserName { get; set; }

        /// <summary>
        /// 客户内码
        /// </summary>
        public string RecordUserInCode { get; set; }

        /// <summary>
        /// 客户码
        /// </summary>
        public string RecordUserCode { get; set; }

        /// <summary>
        /// 档案归属支行
        /// </summary>
        public long? ManagerDepartmentId { get; set; }
        public DepartmentEntity ManagerDepartment { get; set; }

        /// <summary>
        /// 档案归属客户经理
        /// </summary>
        public long? ManagerUserId { get; set; }
        public UserEntity ManagerUser { get; set; }

        /// <summary>
        /// 档案类型
        /// </summary>
        public int RecordType { get; set; }

        /// <summary>
        /// 授信到期日
        /// </summary>
        public DateTime? CreditDueDate { get; set; }

        /// <summary>
        /// 档案状态
        /// </summary>
        public int Status { get; set; }
    }
}
