using System;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    [Table(Name ="ad_userdepartment")]
    public class UserDepartmentEntity:EntityAdd
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public DepartmentEntity Department { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public UserEntity User { get; set; }
    }
}
