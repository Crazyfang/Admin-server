using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Admin.Department.Input;

namespace Admin.Core.Service.Admin.Department
{
    public partial interface IDepartmentService
    {
        Task<IResponseOutput> GetAsync(long id);

        Task<IResponseOutput> ListAsync(string key);

        Task<IResponseOutput> UpdateAsync(DepartmentUpdateInput input);

        Task<IResponseOutput> AddAsync(DepartmentUpdateInput input);

        //Task<IResponseOutput> UpdateAsync(DepartmentEntity department);

        //Task<IResponseOutput> InsertAsync(DepartmentEntity department);

        //Task<IResponseOutput> ListAsync(string key, DateTime? start, DateTime? end);

        Task<IResponseOutput> DeleteAsync(long id);

        Task<IResponseOutput> SoftDeleteAsync(long id);

        Task<IResponseOutput> GetSelectListAsync(long id);
    }
}
