using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.RecordFile.Input;
using Admin.Core.Service.Record.RecordFile.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordFile
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<RecordFileAddInput, RecordFileEntity>();
            CreateMap<RecordFileUpdateInput, RecordFileEntity>();
            CreateMap<RecordFileEntity, RecordFileListOutput>();
            CreateMap<RecordFileEntity, AddRecordFileOutput>().ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.RecordFileName)
            ).ForMember(
                i => i.RecordFileId,
                m => m.MapFrom(n => n.Id)
            ).ReverseMap();
            CreateMap<RecordFileEntity, RecordFileUpdateOutput>().ForMember(
                i => i.RecordFileId,
                m => m.MapFrom(n => n.Id)
            ).ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.RecordFileName)
            ).ReverseMap();
            CreateMap<RecordFileUpdateOutput, CheckedRecordFileEntity>();

            CreateMap<RecordFileEntity, RecordFileAdditionalOuput>().ForMember(
                i => i.RecordFileId,
                m => m.MapFrom(n => n.Id)
            ).ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.RecordFileName)
            ).ForMember(
                i => i.Id,
                m => m.Ignore()
            );
            CreateMap<RecordFileAdditionalOuput, CheckedRecordFileEntity>();
        }
    }
}
