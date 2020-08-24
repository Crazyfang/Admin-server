using System;
namespace Admin.Core.Service.Admin.User.Input
{
    public class UserSearchInput
    {
        public string UserName { get; set; }

        public long? DepartmentId { get; set; }

        public long? RoleId { get; set; }
    }
}
