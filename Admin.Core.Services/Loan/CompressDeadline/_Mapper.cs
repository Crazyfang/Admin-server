using System;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.CompressDeadline.Input;
using AutoMapper;

namespace Admin.Core.Service.Loan.CompressDeadline
{
    public class _Mapper:Profile
    {
        public _Mapper()
        {
            CreateMap<CompressDeadlineAddInput, CompressDeadlineEntity>();
        }
    }
}
