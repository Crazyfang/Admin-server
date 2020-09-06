using System;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.CompressType.Input;
using Admin.Core.Service.Loan.CompressType.Output;
using AutoMapper;

namespace Admin.Core.Service.Loan.CompressType
{
    public class _Mapper:Profile
    {
        public _Mapper()
        {
            CreateMap<CompressTypeAddInput, CompressTypeEntity>().ForMember(
                m => m.CompressName,
                n => n.MapFrom(i => i.Name)
            ).ForMember(
                m => m.TargetValue,
                n => n.MapFrom(i => i.WantedValue)
            );
            CreateMap<CompressTypeEntity, CompressTypeInfoOutput>().ForMember(
                m => m.Name,
                n => n.MapFrom(i => i.CompressName)
            ).ForMember(
                m => m.WantedValue,
                n => n.MapFrom(i => i.TargetValue)
            );

            CreateMap<CompressTypeEntity, CompressTypeDetailOutput>().ForMember(
                m => m.Name,
                n => n.MapFrom(i => i.CompressName)
            ).ForMember(
                m => m.WantedValue,
                n => n.MapFrom(i => i.TargetValue)
            ).ForMember(
                m => m.Checked,
                n => n.MapFrom(i => true)
            );
        }
    }
}
