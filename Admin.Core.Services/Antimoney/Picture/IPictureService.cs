using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Service.Antimoney.Picture.Output;

namespace Admin.Core.Service.Antimoney.Picture
{
    public partial interface IPictureService
    {
        /// <summary>
        /// 返回文件下面的图片列表
        /// </summary>
        /// <param name="fileId">文件主键</param>
        /// <returns></returns>
        Task<IResponseOutput> ReturnPictureListByFileIdAsync(long fileId);

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="input">图片列表</param>
        /// <param name="fileId">文件主键</param>
        /// <returns></returns>
        Task<IResponseOutput> AddOrEditPictureAsync(List<PictureListOutput> input, long fileId);
    }
}
