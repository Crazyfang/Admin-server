using System;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.Contract.Input;
using Admin.Core.Service.Antimoney.Contract.Output;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.Contract
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<ContractEntity, ContractPageOutput>().ForMember(
                    i => i.Currency,
                    j => j.MapFrom(k => k.Currency.CurrencyName)
                );
            CreateMap<ContractAddInput, ContractEntity>();
            CreateMap<ContractEditInput, ContractEntity>().ReverseMap();
            CreateMap<ContractEntity, ContractNoticeInput>()
                .ForMember(
                    i => i.ContractId,
                    j => j.MapFrom(k => k.Id)
                );
        }
    }
}
