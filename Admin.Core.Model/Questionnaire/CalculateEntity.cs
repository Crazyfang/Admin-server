using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Questionnaire
{
    [Table(Name = "qn_calculate")]
    public class CalculateEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 关联户主表
        /// </summary>
        public string HouseHoldId { get; set; }

        public HouseHoldEntity HouseHold { get; set; }

        /// <summary>
        /// 建议授信额
        /// </summary>
        public float SuggestValue { get; set; }

        /// <summary>
        /// 建议授信人
        /// </summary>
        public long HouseHoldMemberId { get; set; }

        public HouseHoldMemberEntity HouseHoldMember { get; set; }
    }
}

