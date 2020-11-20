using System;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.Currency.Output;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.Currency
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<CurrencyEntity, CurrencyListOutput>().ForMember(
                    i => i.Label,
                    j => j.MapFrom(k => k.CurrencyName)
                ).ForMember(
                    i => i.Value,
                    j => j.MapFrom(k => k.Id)
                );
        }
    }
}
