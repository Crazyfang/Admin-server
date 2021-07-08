using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Antimoney.Picture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Antimoney
{
    public class PictureController:AreaController
    {
        private readonly IPictureService _pictureService;
        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        /// <summary>
        /// 根据文件主键获取
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long fileId)
        {
           return await _pictureService.ReturnPictureListByFileIdAsync(fileId);
        }

        /// <summary>
        /// 根据合同主键和文件主键返回文件列表
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPictureList(long contractId, long fileId)
        {
            return await _pictureService.ReturnPictureListByContractIdAsync(contractId, fileId);
        }
    }
}
