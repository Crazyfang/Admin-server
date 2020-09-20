using System;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Service.Questionnaire.HouseHoldMember.Input;
using Admin.Core.Service.Questionnaire.HouseHoldMember.Output;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.HouseHoldMember
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<HouseHoldMemberEntity, HouseHoldMemberAddOutput>().ForMember(
                i => i.UserName,
                j => j.MapFrom(n => n.MemberName)
            ).ForMember(
                i => i.LongStay,
                j => j.MapFrom(n => false)
            ).ForMember(
                i => i.Relation,
                j => j.MapFrom(n => n.Relationship)
            );

            CreateMap<HouseHoldMemberAddInput, MemberResidenceEntity>().ForMember(
                i => i.HouseHoldMemberId,
                j => j.MapFrom(n => n.Id)
            ).ForMember(
                i => i.Residence,
                j => j.MapFrom(n => n.LongStay)
            ).ForMember(
                i => i.Id,
                j => j.Ignore()
            );
        }
    }
}
