using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.RecordType.Input;
using Admin.Core.Service.Record.RecordType.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordType
{
    public class MapConfig:Profile
    {
        public MapConfig()
        {
            CreateMap<RecordTypeEntity, RecordTypeListOutput>();
            CreateMap<RecordTypeAddInput, RecordTypeEntity>();
            CreateMap<RecordTypeUpdateInput, RecordTypeEntity>();
        }
    }
}
