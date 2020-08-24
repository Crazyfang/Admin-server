using System;
using System.Collections.Generic;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_recordborrow")]
    public class RecordBorrowEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public UserEntity User { get; set; }

        [Navigate(nameof(RecordBorrowItemEntity.RecordBorrowId))]
        public List<RecordBorrowItemEntity> RecordBorrowItemList { get; set; }

        public List<long> RecordBorrowItemIdList { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string RefuseReason { get; set; }

        /// <summary>
        /// 借阅类型
        /// 0-借阅
        /// 1-调阅
        /// </summary>
        public int BorrowType { get; set; }

        /// <summary>
        /// 完成标志
        /// 0-未完成
        /// 1-完成
        /// 2-拒绝
        /// 3-取消申请
        /// 4-归还
        /// </summary>
        public int ReturnSign { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}
