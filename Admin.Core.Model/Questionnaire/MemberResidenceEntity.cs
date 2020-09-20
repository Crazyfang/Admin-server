using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Questionnaire
{
    /// <summary>
    /// 评价户主成员常住表
    /// </summary>
    [Table(Name = "qn_memberresidence")]
    public class MemberResidenceEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 关联评价表
        /// </summary>
        public long AppraiseId { get; set; }

        public AppraiseEntity Appraise { get; set; }

        public HouseHoldMemberEntity HouseHoldMember { get; set; }

        /// <summary>
        /// 关联成员表
        /// </summary>
        public long HouseHoldMemberId { get; set; }

        /// <summary>
        /// 是否常住
        /// </summary>
        public bool Residence { get; set; }
    }
}
