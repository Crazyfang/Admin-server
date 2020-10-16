using System;
using System.Collections.Generic;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Loan
{
    [Table(Name = "ln_loanuser")]
    public class LoanUserEntity
    {
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
        /// 起始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 审核用户
        /// </summary>
        public long? VerifyUserId { get; set; }

        public UserEntity VerifyUser { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? VerifyTime { get; set; }

        /// <summary>
        /// 完成标记
        /// 0-未完成
        /// 1-完成
        /// 2-审核通过
        /// 3-超期审核通过
        /// </summary>
        public int OverSign { get; set; }

        public List<CompressDeadlineEntity> Budget { get; set; }
    }
}
