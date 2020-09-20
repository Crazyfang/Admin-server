using System;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Service.Questionnaire.Appraise.Input;
using Admin.Core.Service.Questionnaire.Appraise.Output;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.Appraise
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<AppraiseEntity, AppraiseAddOutput>();
            CreateMap<AppraiseAddInput, AppraiseEntity>();
            CreateMap<AppraiseEntity, AppraisePageOutput>().ForMember(
                i => i.Address,
                j => j.MapFrom(m => m.HouseHold.HeadUserAddress)
            ).ForMember(
                i => i.IdNumber,
                j => j.MapFrom(m => m.HouseHold.HeadUserIdNumber)
            ).ForMember(
                i => i.UserName,
                j => j.MapFrom(m => m.HouseHold.HeadUserName)
            ).ForMember(
                i => i.HouseHoldId,
                j => j.MapFrom(m => m.HouseHold.Id)
            );

            CreateMap<AppraiseEntity, AppraiseDetailOutput>();
        }
    }
}
