using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Repository.Antimoney.File;
using Admin.Core.Service.Antimoney.File.Input;
using AutoMapper;
using System.Linq;
using Admin.Core.Service.Antimoney.Picture;
using Admin.Core.Repository.Antimoney.Picture;
using Admin.Core.Service.Antimoney.Picture.Output;

namespace Admin.Core.Service.Antimoney.File
{
    public class FileService:IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IPictureService _pictureService;
        private readonly IPictureRepository _pictureRepository;
        public FileService(IFileRepository fileRepository
            , IMapper mapper
            , IPictureService pictureService
            , IPictureRepository pictureRepository)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
            _pictureService = pictureService;
            _pictureRepository = pictureRepository;
        }

        [Transaction]
        public async Task<IResponseOutput> AddFileAsync(IList<FileAddOrEditInput> input)
        {
            var entitys = _mapper.Map<List<FileEntity>>(input);

            foreach(var item in entitys)
            {
                item.ContractId = entitys[0].ContractId;
                if(item.Id == 0)
                {
                    var file = await _fileRepository.InsertAsync(item);
                    foreach(var pic in item.PictureList)
                    {
                        pic.FileId = file.Id;
                        await _pictureRepository.InsertAsync(pic);
                    }
                }
                else
                {
                    await _fileRepository.UpdateAsync(item);
                    var result = _mapper.Map<List<PictureListOutput>>(item.PictureList);
                    await _pictureService.AddOrEditPictureAsync(result, item.Id);
                }
            }
            var idList = entitys.Select(i => i.Id).ToList();
            var deleteIdList = await _fileRepository.Select.Where(i => !idList.Contains(i.Id) && i.ContractId == input[0].ContractId).ToListAsync(i => i.Id);
            foreach(var item in deleteIdList)
            {
                await _fileRepository.DeleteAsync(i => i.Id == item);
                await _pictureRepository.DeleteAsync(i => i.FileId == item);
            }
            

            return ResponseOutput.Ok();
        }
    }
}
