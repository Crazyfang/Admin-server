using System;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.CompressDeadline.Input;
using Admin.Core.Service.Loan.CompressDeadline.Output;
using AutoMapper;

namespace Admin.Core.Service.Loan.CompressDeadline
{
    public class _Mapper:Profile
    {
        public _Mapper()
        {
            CreateMap<CompressDeadlineAddInput, CompressDeadlineEntity>();
            CreateMap<CompressDeadlineEntity, CompressDeadlineInfoOutput>();
            CreateMap<CompressDeadlineEntity, CompressDeadlineDetailOutput>().ForMember(
                m => m.CountMonth,
                n => n.MapFrom(i => (i.EndTime.Year - i.BeginTime.Year) * 12 + i.EndTime.Month - i.BeginTime.Month)
            );
        }
    }
}
