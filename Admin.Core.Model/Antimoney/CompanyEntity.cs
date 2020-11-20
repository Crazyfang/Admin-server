using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using FreeSql.DataAnnotations;
namespace Admin.Core.Model.Antimoney
{
    [Table(Name = "am_company")]
    public class CompanyEntity:EntityBase
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 营业执照号码
        /// </summary>
        public string BusinessLicense { get; set; }

        [Navigate(nameof(ContractEntity.CompanyId))]
        public IList<ContractEntity> Contracts { get; set; }
    }
}
