using System;
namespace Admin.Core.Service.Record.Record.Input
{
    public class RecordUpdateInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 档案编号
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// 档案用户姓名
        /// </summary>
        public string RecordUserName { get; set; }

        /// <summary>
        /// 客户内码
        /// </summary>
        public string RecordUserInCode { get; set; }

        /// <summary>
        /// 客户码
        /// </summary>
        public string RecordUserCode { get; set; }

        /// <summary>
        /// 档案归属支行
        /// </summary>
        public long? ManagerDepartmentId { get; set; }

        /// <summary>
        /// 档案归属客户经理
        /// </summary>
        public long? ManagerUserId { get; set; }

        /// <summary>
        /// 档案类型
        /// </summary>
        public int RecordType { get; set; }

        /// <summary>
        /// 授信到期日
        /// </summary>
        public DateTime CreditDueDate { get; set; }

        /// <summary>
        /// 档案状态
        /// </summary>
        public int Status { get; set; }
    }
}
