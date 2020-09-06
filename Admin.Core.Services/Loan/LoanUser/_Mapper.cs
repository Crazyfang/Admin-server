using System;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.LoanUser.Input;
using Admin.Core.Service.Loan.LoanUser.Output;
using AutoMapper;

namespace Admin.Core.Service.Loan.LoanUser
{
    public class _Mapper:Profile
    {
        public _Mapper()
        {
            CreateMap<LoanUserAddInput, LoanUserEntity>().ReverseMap();

            CreateMap<LoanUserEditInput, LoanUserEntity>().ReverseMap();

            CreateMap<LoanUserEntity, LoanUserInfoOutput>().ForMember(
                i => i.VerifyUserName,
                m => m.MapFrom(n => n.VerifyUser == null ? "" : n.VerifyUser.NickName)
            ).ForMember(
                i => i.VerifyUserCode,
                m => m.MapFrom(n => n.VerifyUser == null ? "" : n.VerifyUser.UserName)
            );

            CreateMap<LoanUserEntity, LoanUserDetailOutput>().ReverseMap();
        }
    }
}
