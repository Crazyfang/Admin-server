using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Repository.Antimoney.Picture;
using Admin.Core.Service.Antimoney.Picture.Input;
using Admin.Core.Service.Antimoney.Picture.Output;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.Picture
{
    public class PictureService:IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;
        public PictureService(IPictureRepository pictureRepository
            , IMapper mapper)
        {
            _pictureRepository = pictureRepository;
            _mapper = mapper;
        }

        public async Task<IResponseOutput> ReturnPictureListByFileIdAsync(long fileId)
        {
            var list = await _pictureRepository.Select
                .Where(i => i.FileId == fileId)
                .ToListAsync();

            var output = _mapper.Map<List<PictureListOutput>>(list);

            return ResponseOutput.Ok(output);
        }

        [Transaction]
        public async Task<IResponseOutput> AddOrEditPictureAsync(List<PictureListOutput> input, long fileId)
        {
            var oldPictureIdList = await _pictureRepository.Select
                .Where(i => i.FileId == fileId)
                .ToListAsync(i => i.Id);

            foreach(var item in input.Where(i => i.Id == 0))
            {
                var entity = _mapper.Map<PictureEntity>(item);
                entity.FileId = fileId;
                await _pictureRepository.InsertAsync(entity);
            }
            var pictureIdList = input.Where(i => i.Id != 0).Select(i => i.Id).ToList();

            foreach(var item in oldPictureIdList.Except(pictureIdList).ToList())
            {
                await _pictureRepository.DeleteAsync(item);
            }

            return ResponseOutput.Ok();
        }
    }
}
