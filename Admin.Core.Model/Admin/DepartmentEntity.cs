using System;
using Admin.Core.Common.BaseModel;
using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    [Table(Name = "ad_department")]
    public class DepartmentEntity : EntityBase
    {
        /// <summary>
        /// 部门代码
        /// </summary>
        public long DepartmentCode { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 上级部门
        /// </summary>
        public long? ParentId { get; set; }
        public DepartmentEntity Parent { get; set; }

        [Navigate(ManyToMany = typeof(UserDepartmentEntity))]
        public ICollection<UserEntity> Users { get; set; }
    }
}
