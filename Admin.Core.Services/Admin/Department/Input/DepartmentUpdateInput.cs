using System;
namespace Admin.Core.Service.Admin.Department.Input
{
    public class DepartmentUpdateInput
    {
        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string DepartmentName { get; set; }

        public long DepartmentCode { get; set; }
    }
}
