using System;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;
namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_recordborrowitem")]
    public class RecordBorrowItemEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        public RecordEntity Record { get; set; }

        public long RecordId { get; set; }

        public long RecordBorrowId { get; set; }

        public RecordBorrowEntity RecordBorrow { get; set; }
    }
}
