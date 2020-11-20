using System;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.Picture.Input;
using Admin.Core.Service.Antimoney.Picture.Output;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.Picture
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<PictureAddInput, PictureEntity>().ForMember(
                    i => i.PictureName,
                    j => j.MapFrom(k => k.Name)
                ).ReverseMap();
            CreateMap<PictureEntity, PictureListOutput>().ForMember(
                    i => i.Name,
                    j => j.MapFrom(k => k.PictureName)
                ).ReverseMap();
        }
    }
}
