using System;
namespace Admin.Core.Service.Record.RecordHistory.Output
{
    public class RecordHistoryGetOutput
    {
        public long Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedUserName { get; set; }

        public string CreatedUserId { get; set; }

        public string OperateType { get; set; }

        public string OperateInfo { get; set; }
    }
}
