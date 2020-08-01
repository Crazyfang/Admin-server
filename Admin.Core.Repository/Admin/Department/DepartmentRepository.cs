using System;
using FreeSql;
using Admin.Core.Model.Admin;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository.Admin.Department
{

    public class DepartmentRepository : RepositoryBase<DepartmentEntity>, IDepartmentRepository
    {
        public DepartmentRepository(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
