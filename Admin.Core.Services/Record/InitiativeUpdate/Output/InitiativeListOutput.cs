using System;
namespace Admin.Core.Service.Record.Record.InitiativeUpdate.Output
{
    public class InitiativeListOutput
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string UserCode { get; set; }

        public int Type { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentCode { get; set; }

        public string RecordUserName { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
