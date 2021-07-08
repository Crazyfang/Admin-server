using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Repository.Antimoney.File;
using Admin.Core.Repository.Antimoney.Picture;
using Admin.Core.Repository.Antimoney.PresetFile;
using Admin.Core.Service.Antimoney.Picture.Input;
using Admin.Core.Service.Antimoney.Picture.Output;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.Picture
{
    public class PictureService:IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        public PictureService(IPictureRepository pictureRepository
            , IFileRepository fileRepository
            , IMapper mapper)
        {
            _pictureRepository = pictureRepository;
            _fileRepository = fileRepository;
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

        public async Task<IResponseOutput> ReturnPictureListByContractIdAsync(long contractId, long fileId)
        {
            var list = await _fileRepository.Select
                .Where(i => i.ContractId == contractId)
                .IncludeMany(i => i.PictureList)
                .ToListAsync();

            var index = list.FindIndex(i => i.Id == fileId);
            if (index != 0)
            {
                var tempLeft = list.Take(index);
                var tempRight = list.Skip(index).Take(list.Count - index);
                list = tempRight.Concat(tempLeft).ToList();
            }

            var output = list.SelectMany(i => i.PictureList).Select(i => i.Url).ToList();

            return ResponseOutput.Ok(output);
        }
    }
}
