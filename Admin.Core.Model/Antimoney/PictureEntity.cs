using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Antimoney
{
    [Table(Name = "am_picture")]
    public class PictureEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        public string PictureName { get; set; }

        public string Url { get; set; }

        public long FileId { get; set; }

        public FileEntity File { get; set; }
    }
}
