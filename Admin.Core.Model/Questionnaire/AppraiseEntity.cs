using System;
using System.Collections.Generic;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Questionnaire
{
    /// <summary>
    /// 评价表
    /// </summary>
    [Table(Name = "qn_appraise")]
    public class AppraiseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 评价人主键
        /// </summary>
        public long AppraiserId { get; set; }

        public UserEntity Appraiser { get; set; }

        /// <summary>
        /// 关联户主表
        /// </summary>
        [Column(StringLength = 20)]
        public string HouseHoldId { get; set; }

        public HouseHoldEntity HouseHold { get; set; }

        /// <summary>
        /// 关联户口人员常住表
        /// </summary>
        [Navigate(nameof(MemberResidenceEntity.AppraiseId))]
        public IList<MemberResidenceEntity> Members { get; set; }

        /// <summary>
        /// 家庭是否有无固定收入情况
        /// </summary>
        public bool Instability { get; set; }

        /// <summary>
        /// 家庭成员欠债不还或有赖账
        /// </summary>
        public bool Repudiate { get; set; }

        /// <summary>
        /// 家庭成员品德较差
        /// </summary>
        public bool Reputation { get; set; }

        /// <summary>
        /// 家庭成员涉嫌赌博行为
        /// </summary>
        public bool Gamble { get; set; }

        /// <summary>
        /// 家庭成员存在民间借贷
        /// </summary>
        public bool Lending { get; set; }

        /// <summary>
        /// 自建房情况
        /// </summary>
        public bool SelfBuilding { get; set; }

        /// <summary>
        /// 平房
        /// </summary>
        public bool Bungalow { get; set; }

        /// <summary>
        /// 楼房
        /// </summary>
        public bool Building { get; set; }

        /// <summary>
        /// 别墅
        /// </summary>
        public bool Cottage { get; set; }

        /// <summary>
        /// 平房数量
        /// </summary>
        public int BungalowCount { get; set; }

        /// <summary>
        /// 楼房数量
        /// </summary>
        public int BuildingCount{ get; set; }

        /// <summary>
        /// 别墅数量
        /// </summary>
        public int CottageCount { get; set; }

        /// <summary>
        /// 商品房持有
        /// </summary>
        public bool GoodsBuilding { get; set; }

        /// <summary>
        /// 商品房数量
        /// </summary>
        public int GoodsBuildingCount { get; set; }

        /// <summary>
        /// 汽车持有
        /// </summary>
        public bool CarHold { get; set; }

        /// <summary>
        /// 汽车持有数量
        /// </summary>
        public int CarHoldCount { get; set; }

        /// <summary>
        /// 负债情况
        /// </summary>
        public float? DebtCondition { get; set; }

        /// <summary>
        /// 家庭收入
        /// </summary>
        public float? HomeEarning { get; set; }

        /// <summary>
        /// 家庭支出
        /// </summary>
        public float? HomePay { get; set; }

        /// <summary>
        /// 存在家庭收入不稳定的情况
        /// </summary>
        public bool Condition1 { get; set; }

        /// <summary>
        /// 家庭存在严重疾病等重大负担
        /// </summary>
        public bool Condition2 { get; set; }

        /// <summary>
        /// 存在家庭关系不和睦、邻里关系紧张、与村民关系不融洽等情况
        /// </summary>
        public bool Condition3 { get; set; }

        /// <summary>
        /// 存在不遵守村规民约、不配合村组织工作开展的情况
        /// </summary>
        public bool Condition4 { get; set; }

        /// <summary>
        /// 存在不守信、恶意拖欠水电气费等不诚信行为
        /// </summary>
        public bool Condition5 { get; set; }

        /// <summary>
        /// 存在其他工作及生活情况需要重点说明的
        /// </summary>
        public string Remarks { get; set; }
    }
}
