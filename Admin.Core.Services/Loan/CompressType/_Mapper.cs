using System;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.CompressType.Input;
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
        }
    }
}
