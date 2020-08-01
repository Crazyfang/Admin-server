using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.OtherRecordFile.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.OtherRecordFile
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<OtherRecordFileEntity, OtherRecordFileOutput>().ForMember(
                i => i.Name,
                n => n.MapFrom(m => m.OtherFileName)
            ).ReverseMap();
        }
    }
}
