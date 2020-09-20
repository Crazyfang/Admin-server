using System;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Service.Questionnaire.MemberResidence.Output;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.MemberResidence
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<MemberResidenceEntity, MemberResidenceDetailOutput>().ForMember(
                i => i.Address,
                j => j.MapFrom(n => n.HouseHoldMember.Address)
            ).ForMember(
                i => i.UserName,
                j => j.MapFrom(n => n.HouseHoldMember.MemberName)
            ).ForMember(
                i => i.IdNumber,
                j => j.MapFrom(n => n.HouseHoldMember.IdNumber)
            ).ForMember(
                i => i.LongStay,
                j => j.MapFrom(n => n.Residence)
            ).ForMember(
                i => i.Relation,
                j => j.MapFrom(n => n.HouseHoldMember.Relationship)
            );
        }
    }
}
