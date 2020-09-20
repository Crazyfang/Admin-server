using System;
using Admin.Core.Model.Questionnaire;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.SectionCode
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<SectionCodeEntity, SectionCodeListOutput>().ForMember(
                i => i.Label,
                j => j.MapFrom(m => m.VillageName)
            ).ForMember(
                i => i.Value,
                j => j.MapFrom(m => m.VillageCode)
            );
        }
    }
}
