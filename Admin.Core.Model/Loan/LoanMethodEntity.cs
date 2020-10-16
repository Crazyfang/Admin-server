using System;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Loan
{
    [Table(Name = "lm_loanmethod")]
    public class LoanMethodEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客户内码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        public long? VerifyUserId { get; set; }

        public UserEntity VerifyUser { get; set; }

        public DateTime? VerifyTime { get; set; }

        /// <summary>
        /// 完成标识
        /// 0-未完成
        /// 1-完成
        /// 2-审核完成
        /// 3-超期完成
        /// 4-超期审核完成
        /// </summary>
        public int OverSign { get; set; }

        /// <summary>
        /// 天数
        /// </summary>
        public int CountDay { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime OverTime { get; set; }

        /// <summary>
        /// 合同号
        /// </summary>
        public string ContractNo { get; set; }

        /// <summary>
        /// 合同总金额
        /// </summary>
        [Column(DbType = "decimal(18,2)")]
        public decimal SumValue { get; set; }

        /// <summary>
        /// 需要抵押替换的金额
        /// </summary>
        [Column(DbType = "decimal(18,2)")]
        public decimal PartialValue { get; set; }
    }
}
