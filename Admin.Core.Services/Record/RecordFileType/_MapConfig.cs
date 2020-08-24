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
            ).ReverseMap();
            CreateMap<RecordFileTypeEntity, RecordFileTypeAddOutput>().ForMember(
                i => i.Children,
                m => m.MapFrom(n => n.RecordFileList)
            ).ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.FileTypeName)
            ).ForMember(
                i => i.RecordFileTypeId,
                m => m.MapFrom(n => n.Id)
            );

            CreateMap<RecordFileTypeEntity, RecordFileTypeUpdateOutput>().ForMember(
                i => i.Children,
                m => m.MapFrom(n => n.RecordFileList)
            ).ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.FileTypeName)
            ).ForMember(
                i => i.RecordFileTypeId,
                m => m.MapFrom(n => n.Id)
            );

            CreateMap<RecordFileTypeOutput, CheckedRecordFileTypeEntity>().ForMember(
                i => i.RecordFileTypeId,
                m => m.MapFrom(n => n.RecordFileTypeId)
            ).ForMember(
                i => i.Id,
                m => m.MapFrom(n => 0)
            );

            CreateMap<RecordFileTypeEntity, RecordFileTypeAdditionalOutput>().ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.FileTypeName)
            ).ForMember(
                i => i.RecordFileTypeId,
                m => m.MapFrom(n => n.Id)
            );

            CreateMap<RecordFileTypeAdditionalOutput, CheckedRecordFileTypeEntity>().ForMember(
                i => i.CheckedRecordFileList,
                m => m.MapFrom(n => n.Children)
            );
        }
    }
}
