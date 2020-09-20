using System;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Service.Questionnaire.HouseHold.Output;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.HouseHold
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<HouseHoldEntity, HouseHoldPageOutput>().ForMember(
                i => i.UserName,
                j => j.MapFrom(m => m.HeadUserName)
            ).ForMember(
                i => i.IdNumber,
                j => j.MapFrom(m => m.HeadUserIdNumber)
            ).ForMember(
                i => i.Address,
                j => j.MapFrom(m => m.HeadUserAddress)
            );

            CreateMap<HouseHoldEntity, HouseHoldCalculateOutput>().ForMember(
                i => i.SuggestCreditorName,
                j => j.MapFrom(m => m.SuggestCreditor.MemberName)
            ).ForMember(
                i => i.SuggestCreditorIdNumber,
                j => j.MapFrom(m => m.SuggestCreditor.IdNumber)
            );
        }
    }
}
