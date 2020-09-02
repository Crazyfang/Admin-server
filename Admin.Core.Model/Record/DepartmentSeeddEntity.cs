using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_departmentseed")]
    public class DepartmentSeeddEntity
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [Column(IsPrimary = true)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门代码
        /// </summary>
        public int DepartmentCode { get; set; }

        /// <summary>
        /// 增长种子
        /// </summary>
        public int Seed { get; set; }
    }
}
