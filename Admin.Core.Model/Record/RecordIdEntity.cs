using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_reocrdid")]
    public class RecordIdEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        public string msg { get; set; }
    }
}
