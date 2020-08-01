using System;
using System.Collections.Generic;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.CheckedRecordFile.Input;
using Admin.Core.Service.Record.RecordFileContract.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.CheckedRecordFile
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<RecordFileEntity, CheckedRecordFileInput>().ForMember(
                i => i.Num,
                m => m.MapFrom(n => 1)
            ).ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.RecordFileName)
            ).ForMember(
                i => i.RecordFileId,
                m => m.MapFrom(n => n.Id)
            ).ForMember(
                i => i.Id,
                m => m.Ignore()
            );
            CreateMap<CheckedRecordFileEntity, CheckedRecordFileInput>().ForMember(
                i => i.Checked,
                m => m.MapFrom(n => true)
            ).ForMember(
                i => i.Name,
                m => m.MapFrom(n => n.RecordFile.RecordFileName)
            );
            CreateMap<CheckedRecordFileInput, CheckedRecordFileEntity>();
        }
    }
}
