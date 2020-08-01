using System;
using System.Collections.Generic;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.RecordFileType.Input;
using Admin.Core.Service.Record.RecordFileType.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordFileType
{
    public class MapConfig:Profile
    {
        public MapConfig()
        {
            CreateMap<RecordFileTypeAddInput, RecordFileTypeEntity>().ForMember(
                m => m.FileTypeName,
                i => i.MapFrom(s => s.RecordFileTypeName)
            );
            CreateMap<RecordFileTypeEntity, RecordFileTypeListOutput>();
            CreateMap<RecordFileTypeUpdateInput, RecordFileTypeEntity>().ForMember(
                m => m.FileTypeName,
                i => i.MapFrom(s => s.RecordFileTypeName)
            );
            CreateMap<RecordFileTypeEntity, RecordFileTypeOutput>().ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.FileTypeName)
            ).ForMember(
                i => i.Other,
                m => m.MapFrom(n => n.OtherRecordFileList)
            ).ReverseMap();
            CreateMap<RecordFileTypeEntity, RecordFileTypeAddOutput>().ForMember(
                i => i.Children,
                m => m.MapFrom(n => n.RecordFileList)
            ).ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.FileTypeName)
            );
        }
    }
}
