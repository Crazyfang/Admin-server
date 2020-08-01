using System.Linq;
using AutoMapper;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.User.Input;
using Admin.Core.Service.Admin.User.Output;

namespace Admin.Core.Service.Admin.User
{
    /// <summary>
    /// 映射配置
    /// 双向映射 .ReverseMap()
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //新增
            CreateMap<UserAddInput, UserEntity>();
            CreateMap<UserUpdateInput, UserEntity>();

            //修改
            CreateMap<UserChangePasswordInput, UserEntity>();
            CreateMap<UserUpdateBasicInput, UserEntity>();

            //查询
            CreateMap<UserEntity, UserGetOutput>().ForMember(
                d => d.RoleIds,
                m => m.MapFrom(s => s.Roles.Select(a => a.Id))
            ).ForMember(
                d => d.DepartmentIds,
                m => m.MapFrom(s => s.Departments.Select(a => a.Id))
            );

            CreateMap<UserEntity, UserListOutput>().ForMember(
                d => d.RoleNames,
                m => m.MapFrom(s => s.Roles.Select(a => a.Name))
            ).ForMember(
                d => d.DepartmentNames,
                m => m.MapFrom(s => s.Departments.Select(a => a.DepartmentName))
            ).ForMember(
                d => d.DepartmentCodes,
                m => m.MapFrom(s => s.Departments.Select(a => a.DepartmentCode.ToString()))
            );

            CreateMap<UserEntity, UserSelectOutput>().ForMember(
                d => d.Value,
                m => m.MapFrom(s => s.Id)
            ).ForMember(
                d => d.Label,
                m => m.MapFrom(s => s.NickName)
            ).ForMember(
                d => d.PeopleId,
                m => m.MapFrom(s => s.UserName)
            );
        }
    }
}
