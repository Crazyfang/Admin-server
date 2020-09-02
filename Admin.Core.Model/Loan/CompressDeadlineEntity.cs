using System;
using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Loan
{
    [Table(Name = "ln_compressdeadline")]
    public class CompressDeadlineEntity
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public LoanUserEntity User { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndTime { get; set; }

        [Navigate(nameof(CompressTypeEntity.CompressDeadlineId))]
        public List<CompressTypeEntity> Item { get; set; }

        [Column(IsIgnore = true)]
        public int CountMonth { get; set; }
    }
}
