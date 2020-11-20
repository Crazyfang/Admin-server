using System;
namespace Admin.Core.Service.Antimoney.Contract.Input
{
    public class ContractAddInput
    {
        /// <summary>
        ///  合同号
        /// </summary>
        public string ContractNo { get; set; }

        /// <summary>
        /// 汇款人姓名
        /// </summary>
        public string RemitterName { get; set; }

        /// <summary>
        /// 汇款人地址
        /// </summary>
        public string RemitterAddress { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        public string ConsigneeAddress { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public long CurrencyId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 解付日期
        /// </summary>
        public DateTime SettlementDate { get; set; }

        /// <summary>
        /// 款项性质
        /// </summary>
        public string PaymentNature { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 资料提交情况
        /// </summary>
        public string DataSubmitInfo { get; set; }

        /// <summary>
        /// 提单查询日期
        /// </summary>
        public DateTime? LadingInquireDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        public long CompanyId { get; set; }
    }
}
