using System;
using System.Collections;
using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Questionnaire
{
    /// <summary>
    /// 户口表
    /// </summary>
    [Table(Name="qn_household")]
    public class HouseHoldEntity
    {
        /// <summary>
        /// 主键(行政代码+四位序号)
        /// </summary>
        [Column(IsPrimary = true, StringLength = 20)]
        public string Id { get; set; }

        /// <summary>
        /// 户主姓名
        /// </summary>
        public string HeadUserName { get; set; }

        /// <summary>
        /// 户主居住地址
        /// </summary>
        public string HeadUserAddress { get; set; }

        /// <summary>
        /// 户主身份证号
        /// </summary>
        public string HeadUserIdNumber { get; set; }

        /// <summary>
        /// 所属乡镇
        /// </summary>
        public string BelongedStreet { get; set; }

        /// <summary>
        /// 建议授信额
        /// </summary>
        public float? SuggestCreditLimit { get; set; }

        /// <summary>
        /// 建议授信人
        /// </summary>
        public long? SuggestCreditorId { get; set; }

        public HouseHoldMemberEntity SuggestCreditor { get; set; }

        /// <summary>
        /// 偏离度过大告警标记
        /// </summary>
        public bool DeviationMark { get; set; }

        /// <summary>
        /// 出现一户风险情况认定
        /// </summary>
        public bool DangerUserMark { get; set; }

        /// <summary>
        /// 承包土地亩数
        /// </summary>
        public float? Acres { get; set; }

        /// <summary>
        /// 风险客户，不予授信标识
        /// </summary>
        public bool RefuseMark { get; set; }

        /// <summary>
        /// 户口归属人员
        /// </summary>
        [Navigate(nameof(HouseHoldMemberEntity.HouseHoldId))]
        public IList<HouseHoldMemberEntity> Members { get; set; }

        /// <summary>
        /// 评价列表
        /// </summary>
        [Navigate(nameof(AppraiseEntity.HouseHoldId))]
        public IList<AppraiseEntity> Appraises { get; set; }
    }
}
