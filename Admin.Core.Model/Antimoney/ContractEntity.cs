using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;
namespace Admin.Core.Model.Antimoney
{
    [Table(Name = "am_contract")]
    public class ContractEntity:EntityBase
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

        public long CurrencyId { get; set; }

        public CurrencyEntity Currency { get; set; }

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
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 提醒日期
        /// </summary>
        public DateTime? AwakeTime { get; set; }

        /// <summary>
        /// 提醒事项
        /// </summary>
        public string AwakeNotes { get; set; }

        /// <summary>
        /// 资料提交情况
        /// </summary>
        [Column(StringLength = -1)]
        public string DataSubmitInfo { get; set; }

        /// <summary>
        /// 提单查询日期
        /// </summary>
        public DateTime? LadingInquireDate { get; set; }

        /// <summary>
        /// 启用标记
        /// </summary>
        public bool EnableSign { get; set; }

        public long CompanyId { get; set; }

        public CompanyEntity Company { get; set; }

        [Navigate(nameof(FileEntity.ContractId))]
        public IList<FileEntity> Files { get; set; }
    }
}
