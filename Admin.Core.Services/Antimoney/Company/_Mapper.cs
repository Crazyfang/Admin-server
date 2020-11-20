using System;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.Company.Input;
using Admin.Core.Service.Antimoney.Company.Output;
using System.Linq;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.Company
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<CompanyEntity, CompanyPageOutput>().ForMember(
                    i => i.NoticeSign,
                    j => j.MapFrom(k => k.Contracts.Any(h => h.AwakeTime <= DateTime.Now && h.EnableSign))
                );
            CreateMap<CompanyAddInput, CompanyEntity>();
            CreateMap<CompanyEditInput, CompanyEntity>();
            CreateMap<CompanyEntity, CompanyInfoOutput>();
        }
    }
}
