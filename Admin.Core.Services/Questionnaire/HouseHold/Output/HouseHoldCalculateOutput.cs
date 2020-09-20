using System;
namespace Admin.Core.Service.Questionnaire.HouseHold.Output
{
    public class HouseHoldCalculateOutput
    {
        public string Id { get; set; }

        public string HeadUserName { get; set; }

        public string HeadUserIdNumber { get; set; }

        public string HeadUserAddress { get; set; }

        public float SuggestCreditLimit { get; set; }

        public string SuggestCreditorName { get; set; }

        public string SuggestCreditorIdNumber { get; set; }

        /// <summary>
        /// 偏离度过大告警标记
        /// </summary>
        public bool DeviationMark { get; set; }

        /// <summary>
        /// 风险客户，不予授信标识
        /// </summary>
        public bool RefuseMark { get; set; }
    }
}
