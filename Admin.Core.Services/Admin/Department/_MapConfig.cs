using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Department.Output;
using Admin.Core.Service.Admin.Department.Input;

namespace Admin.Core.Service.Admin.Department
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DepartmentListOutput, DepartmentEntity>();
            CreateMap<DepartmentUpdateInput, DepartmentEntity>();
        }
    }
}
