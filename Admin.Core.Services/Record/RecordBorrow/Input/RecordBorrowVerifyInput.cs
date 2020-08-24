using System;
namespace Admin.Core.Service.Record.RecordBorrow.Input
{
    public class RecordBorrowVerifyInput
    {
        public long Id { get; set; }

        /// <summary>
        /// 审核类别
        /// 0-不同意
        /// 1-同意
        /// </summary>
        public int VerifyType{ get; set; }

        /// <summary>
        /// 拒绝意见
        /// </summary>
        public string RefuseReason { get; set; }
    }
}
