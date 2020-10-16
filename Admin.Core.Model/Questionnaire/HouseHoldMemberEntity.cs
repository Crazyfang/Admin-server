using System;
using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Questionnaire
{
    /// <summary>
    /// 户口成员表
    /// </summary>
    [Table(Name = "qn_householdmember")]
    public class HouseHoldMemberEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 成员姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 居住地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 与户主关系
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// 关联户口
        /// </summary>
        [Column(StringLength = 20)]
        public string HouseHoldId { get; set; }

        public HouseHoldEntity HouseHold { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        public string NativePlace { get; set; }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MaritaStatus { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        [Column(StringLength = 30)]
        public string Nationality { get; set; }

        /// <summary>
        /// 受教育程度
        /// </summary>
        [Column(StringLength = 20)]
        public string Education { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string WorkUnit { get; set; }

        /// <summary>
        /// 是否有借记卡
        /// </summary>
        public bool OwnedDebitCard { get; set; }

        /// <summary>
        /// 是否有丰收互联
        /// </summary>
        public bool OwnedHarvestInternet { get; set; }

        [Navigate(nameof(MemberResidenceEntity.HouseHoldMemberId))]
        public List<MemberResidenceEntity> MemberResidences { get; set; }
    }
}
