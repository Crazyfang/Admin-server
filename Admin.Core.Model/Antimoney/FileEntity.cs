using System;
using System.Collections.Generic;
using FreeSql.DataAnnotations;
namespace Admin.Core.Model.Antimoney
{
    [Table(Name = "am_file")]
    public class FileEntity
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 关联合同
        /// </summary>
        public ContractEntity Contract { get; set; }

        public long ContractId { get; set; }

        /// <summary>
        /// 关联预设文件
        /// </summary>
        public PresetFileEntity PresetFile { get; set; }

        public long PresetFileId { get; set; }

        /// <summary>
        /// 完成标记
        /// </summary>
        public bool OverSign { get; set; }

        /// <summary>
        /// 图片列表
        /// </summary>
        [Navigate(nameof(PictureEntity.FileId))]
        public List<PictureEntity> PictureList { get; set; }
    }
}
