using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Questionnaire
{
    [Table(Name = "qn_sectioncode")]
    public class SectionCodeEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        /// <summary>
        /// 行政村名称
        /// </summary>
        public string VillageName { get; set; }

        /// <summary>
        /// 街道代码
        /// </summary>
        [Column(StringLength = 10)]
        public string StreetCode { get; set; }

        /// <summary>
        /// 行政村代码
        /// </summary>
        [Column(StringLength = 10)]
        public string VillageCode { get; set; }
    }
}
