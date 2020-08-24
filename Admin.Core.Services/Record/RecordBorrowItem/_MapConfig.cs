using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.Record.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordBorrowItem
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<RecordBorrowItemEntity, BorrowDetailOutput>().ForMember(
                i => i.RecordUserName,
                m => m.MapFrom(n => n.Record.RecordUserName)
            ).ForMember(
                i => i.RecordUserInCode,
                m => m.MapFrom(n => n.Record.RecordUserInCode)
            ).ForMember(
                i => i.RecordUserCode,
                m => m.MapFrom(n => n.Record.RecordUserCode)
            ).ForMember(
                i => i.Id,
                m => m.MapFrom(n => n.Record.Id)
            ).ForMember(
                i => i.RecordId,
                m => m.MapFrom(n => n.Record.RecordId)
            );
        }
    }
}
