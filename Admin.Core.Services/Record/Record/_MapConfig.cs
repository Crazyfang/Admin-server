using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.Record.Input;
using Admin.Core.Service.Record.Record.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.Record
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<RecordEntity, RecordGetOutput>().ForMember(
                d => d.DepartmentName,
                m => m.MapFrom(s => s.ManagerDepartment.DepartmentName)
            ).ForMember(
                d => d.DepartmentCode,
                m => m.MapFrom(s => s.ManagerDepartment.DepartmentCode)
            ).ForMember(
                d => d.UserName,
                m => m.MapFrom(s => s.ManagerUser.NickName)
            ).ForMember(
                d => d.UserCode,
                m => m.MapFrom(s => s.ManagerUser.UserName)
            );

            CreateMap<RecordEntity, RecordListOutput>().ForMember(
                d => d.DepartmentName,
                m => m.MapFrom(s => s.ManagerDepartment.DepartmentName)
            ).ForMember(
                d => d.DepartmentCode,
                m => m.MapFrom(s => s.ManagerDepartment.DepartmentCode)
            ).ForMember(
                d => d.UserName,
                m => m.MapFrom(s => s.ManagerUser.NickName)
            ).ForMember(
                d => d.UserCode,
                m => m.MapFrom(s => s.ManagerUser.UserName)
            );

            CreateMap<RecordAddInput, RecordEntity>();

            CreateMap<RecordEntity, RecordUpdateInput>().ReverseMap();

            CreateMap<RecordEntity, HandOverRecordPageOutput>().ForMember(
                d => d.DepartmentName,
                m => m.MapFrom(s => s.ManagerDepartment.DepartmentName)
            ).ForMember(
                d => d.DepartmentCode,
                m => m.MapFrom(s => s.ManagerDepartment.DepartmentCode)
            ).ForMember(
                d => d.RecordManagerName,
                m => m.MapFrom(s => s.ManagerUser.NickName)
            ).ForMember(
                d => d.RecordManagerCode,
                m => m.MapFrom(s => s.ManagerUser.UserName)
            );

            CreateMap<RecordBorrowItemEntity, ReturnRecordOutput>().ForMember(
                d => d.Id,
                m => m.MapFrom(s => s.Record.Id)
            ).ForMember(
                d => d.RecordId,
                m => m.MapFrom(s => s.Record.RecordId)
            ).ForMember(
                d => d.RecordUserCode,
                m => m.MapFrom(s => s.Record.RecordUserCode)
            ).ForMember(
                d => d.RecordUserInCode,
                m => m.MapFrom(s => s.Record.RecordUserInCode)
            ).ForMember(
                d => d.RecordUserName,
                m => m.MapFrom(s => s.Record.RecordUserName)
            );

        }
    }
}
