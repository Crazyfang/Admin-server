using System;
using System.Collections.Generic;
using Admin.Core.Service.Loan.CompressDeadline.Output;

namespace Admin.Core.Service.Loan.LoanUser.Output
{
    public class LoanUserInfoOutput
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string UserCode { get; set; }

        public string VerifyUserCode { get; set; }

        public string VerifyUserName { get; set; }

        public DateTime? VerifyTime { get; set; }

        /// <summary>
        /// 完成标记
        /// 0-未完成
        /// 1-完成
        /// 2-审核通过
        /// 3-超期未完成
        /// </summary>
        public int OverSign { get; set; }

        public List<CompressDeadlineInfoOutput> Budget { get; set; }
    }
}
