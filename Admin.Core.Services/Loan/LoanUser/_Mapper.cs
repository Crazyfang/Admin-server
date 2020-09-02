using System;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.LoanUser.Input;
using AutoMapper;

namespace Admin.Core.Service.Loan.LoanUser
{
    public class _Mapper:Profile
    {
        public _Mapper()
        {
            CreateMap<LoanUserAddInput, LoanUserEntity>();
        }
    }
}
