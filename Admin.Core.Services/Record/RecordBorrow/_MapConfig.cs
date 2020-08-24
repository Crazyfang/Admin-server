using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.RecordBorrow.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordBorrow
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<RecordBorrowEntity, RecordBorrowOutput>().ForMember(
                i => i.UserCode,
                m => m.MapFrom(n => n.User.UserName)
            ).ForMember(
                i => i.UserName,
                m => m.MapFrom(n => n.User.NickName)
            ).ForMember(
                i => i.DepartmentCode,
                m => m.MapFrom(n => n.User.Departments.Count > 0 ? n.User.Departments[0].DepartmentCode.ToString() : "无部门编号")
            ).ForMember(
                i => i.DepartmentName,
                m => m.MapFrom(n => n.User.Departments.Count > 0 ? n.User.Departments[0].DepartmentName : "无部门名称")
            ).ForMember(
                i => i.RecordCount,
                m => m.MapFrom(n => n.RecordBorrowItemList.Count)
            );

            CreateMap<RecordBorrowEntity, RecordBorrowDetailOutput>().ForMember(
                i => i.UserCode,
                m => m.MapFrom(n => n.User.UserName)
            ).ForMember(
                i => i.UserName,
                m => m.MapFrom(n => n.User.NickName)
            ).ForMember(
                i => i.DepartmentCode,
                m => m.MapFrom(n => n.User.Departments.Count > 0 ? n.User.Departments[0].DepartmentCode.ToString() : "无部门编号")
            ).ForMember(
                i => i.DepartmentName,
                m => m.MapFrom(n => n.User.Departments.Count > 0 ? n.User.Departments[0].DepartmentName : "无部门名称")
            ).ForMember(
                i => i.RecordBorrowItemList,
                m => m.MapFrom(n => n.RecordBorrowItemList)
            );

            CreateMap<RecordBorrowEntity, ReturnPageOutput>().ForMember(
                i => i.UserCode,
                m => m.MapFrom(n => n.User.UserName)
            ).ForMember(
                i => i.UserName,
                m => m.MapFrom(n => n.User.NickName)
            ).ForMember(
                i => i.DepartmentCode,
                m => m.MapFrom(n => n.User.Departments.Count > 0 ? n.User.Departments[0].DepartmentCode.ToString() : "无部门编号")
            ).ForMember(
                i => i.DepartmentName,
                m => m.MapFrom(n => n.User.Departments.Count > 0 ? n.User.Departments[0].DepartmentName : "无部门名称")
            ).ForMember(
                i => i.Children,
                m => m.MapFrom(n => n.RecordBorrowItemList)
            ).ForMember(
                i => i.RecordCount,
                m => m.MapFrom(n => n.RecordBorrowItemList.Count)
            );
        }
    }
}
