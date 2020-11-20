using System;
using FreeSql.DataAnnotations;
namespace Admin.Core.Model.Antimoney
{
    [Table(Name = "am_presetfile")]
    public class PresetFileEntity
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 文件名字
        /// </summary>
        public string FileName { get; set; }
    }
}
