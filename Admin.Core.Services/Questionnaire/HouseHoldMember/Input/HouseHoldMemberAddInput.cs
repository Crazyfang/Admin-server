using System;
namespace Admin.Core.Service.Questionnaire.HouseHoldMember.Input
{
    public class HouseHoldMemberAddInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 与户主关系
        /// </summary>
        public string Relation { get; set; }

        /// <summary>
        /// 是否常住
        /// </summary>
        public bool LongStay { get; set; }
    }
}
