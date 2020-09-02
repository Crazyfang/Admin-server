using System;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Record
{
    [Table(Name = "rt_notify")]
    public class NotifyEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        /// <summary>
        /// 消息通知
        /// </summary>
        [Column(StringLength = -1)]
        public string Message { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        public UserEntity User { get; set; }

        /// <summary>
        /// 阅读标志
        /// false-未阅读
        /// true-阅读
        /// </summary>
        public bool Sign { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTime? ReadTime { get; set; }
    }
}
