using System;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.File.Input;
using Admin.Core.Service.Antimoney.File.Output;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.File
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<FileAddOrEditInput, FileEntity>();
            CreateMap<PresetFileEntity, FileAddOutput>().ForMember(
                    i => i.PresetFileId,
                    j => j.MapFrom(k => k.Id)
                ).ForMember(
                    i => i.FileName,
                    j => j.MapFrom(k => k.FileName)
                ).ForMember(
                    i => i.Id,
                    j => j.Ignore()
                );

            CreateMap<FileEntity, FileAddOutput>();
        }
    }
}
