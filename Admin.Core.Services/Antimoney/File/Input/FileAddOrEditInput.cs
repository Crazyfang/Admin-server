using System;
using System.Collections.Generic;
using Admin.Core.Service.Antimoney.Picture.Output;

namespace Admin.Core.Service.Antimoney.File.Input
{
    public class FileAddOrEditInput
    {
        public long Id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        public long ContractId { get; set; }

        public long PresetFileId { get; set; }

        public bool OverSign { get; set; }

        public List<PictureListOutput> PictureList { get; set; }
    }
}
