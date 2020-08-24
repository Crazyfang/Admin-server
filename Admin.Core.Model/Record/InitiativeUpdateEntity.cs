using System;
using System.Collections.Generic;
using Admin.Core.Common.BaseModel;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_initiativeupdate")]
    public class InitiativeUpdateEntity: EntityAdd
    {
        /// <summary>
        /// 状态
        /// 0-待审核
        /// 1-审核通过
        /// 2-审核被拒绝
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string RefuseReason { get; set; }

        /// <summary>
        /// 档案主键
        /// </summary>
        public long RecordId { get; set; }

        /// <summary>
        /// 更新类型
        /// 0-过期更新
        /// 1-主动更新
        /// </summary>
        public int Type { get; set; }

        public RecordEntity Record { get; set; }

        [Navigate(nameof(CreatedUserId))]
        public UserEntity ApplyUser { get; set; }

        [Navigate(nameof(InitiativeUpdateItemEntity.InitiativeUpdateId))]
        public List<InitiativeUpdateItemEntity> initiativeUpdateItemEntities { get; set; }
    }
}
