using System;
namespace Admin.Core.Service.Admin.Department.Output
{
    public class DepartmentListOutput
    {
        public long Id { get; set; }

        public string DepartmentName { get; set; }

        public long DepartmentCode { get; set; }

        public long? ParentId { get; set; }
    }
}
