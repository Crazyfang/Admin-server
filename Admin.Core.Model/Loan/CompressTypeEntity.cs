using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Loan
{
    [Table(Name = "ln_compresstype")]
    public class CompressTypeEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        /// <summary>
        /// 压缩归属类别(信用、担保、抵押)
        /// </summary>
        public string CompressName { get; set; }

        /// <summary>
        /// 压缩目标值
        /// </summary>
        public double TargetValue { get; set; }

        /// <summary>
        /// 目前值
        /// </summary>
        public double? PresentValue { get; set; }

        public long CompressDeadlineId { get; set; }

        public CompressDeadlineEntity CompressDeadline { get; set; }

        public long LoadUserId { get; set; }

        public LoanUserEntity LoanUser { get; set; }

        /// <summary>
        /// 完成标记
        /// 0-未完成
        /// 1-完成
        /// </summary>
        public int OverSign { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        [Column(IsIgnore = true)]
        public bool Checked { get; set; }
    }
}
