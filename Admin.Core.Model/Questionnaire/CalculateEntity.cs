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
        /// 授信平均值
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// 方差之和
        /// </summary>
        public double VarianceSum { get; set; }

        /// <summary>
        /// 标准差
        /// </summary>
        public double StandardDeviation { get; set; }

        /// <summary>
        /// 偏离度
        /// </summary>
        public double Deviation { get; set; }

        /// <summary>
        /// 偏离度过大告警标记
        /// </summary>
        public bool DeviationMark { get; set; }

        /// <summary>
        /// 出现一户风险情况认定
        /// </summary>
        public bool DangerUserMark { get; set; }

        /// <summary>
        /// 风险客户，不予授信标识
        /// </summary>
        public bool RefuseMark { get; set; }
    }
}

