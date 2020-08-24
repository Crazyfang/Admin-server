using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.Record.InitiativeUpdate.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.Record.InitiativeUpdate
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<InitiativeUpdateEntity, InitiativeListOutput>().ForMember(
                i => i.UserCode,
                m => m.MapFrom(n => n.ApplyUser.UserName)
            ).ForMember(
                i => i.UserName,
                m => m.MapFrom(n => n.ApplyUser.NickName)
            ).ForMember(
                i => i.DepartmentCode,
                m => m.MapFrom(n => n.ApplyUser.Departments.Count > 0 ? n.ApplyUser.Departments[0].DepartmentCode : 0)
            ).ForMember(
                i => i.DepartmentName,
                m => m.MapFrom(n => n.ApplyUser.Departments.Count > 0 ? n.ApplyUser.Departments[0].DepartmentName : "无")
            ).ForMember(
                i => i.RecordUserName,
                m => m.MapFrom(n => n.Record.RecordUserName)
            );
        }
    }
}
