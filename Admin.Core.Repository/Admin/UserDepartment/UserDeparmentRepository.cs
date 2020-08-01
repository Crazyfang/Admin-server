using System;
using Admin.Core.Common.Auth;
using Admin.Core.Model.Admin;
using FreeSql;

namespace Admin.Core.Repository.Admin.UserDepartment
{
    public class UserDeparmentRepository:RepositoryBase<UserDepartmentEntity>, IUserDepartmentRepository
    {
        public UserDeparmentRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
