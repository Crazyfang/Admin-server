using System;
using Admin.Core.Model.Record;
using Admin.Core.Service.Record.RecordFileContract.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordFileContract
{
    public class _MapConfig:Profile
    {
        public _MapConfig()
        {
            CreateMap<RecordFileContractEntity, RecordFileContractOutput>().ReverseMap();
        }
    }
}
