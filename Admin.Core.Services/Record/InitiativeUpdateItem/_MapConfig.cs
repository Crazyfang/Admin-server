using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.CheckedRecordFile.Input;
using Admin.Core.Service.Record.InitiativeUpdateItem.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.Record.InitiativeUpdateItem
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<CheckedRecordFileInput, InitiativeUpdateItemEntity>().ForMember(
                i => i.DelSign,
                m => m.MapFrom(n => n.Checked)
            ).ForMember(
                i => i.Id,
                m => m.Ignore()
            );

            CreateMap<InitiativeUpdateItemEntity, InitiativeUpdateItemOutput>().ForMember(
                i => i.Num,
                m => m.MapFrom(n => n.CheckedRecordFile.Num)
            ).ForMember(
                i => i.FileName,
                m => m.MapFrom(n => n.CheckedRecordFile.Name)
            );
        }
    }
}
