using System;
namespace Admin.Core.Service.Record.InitiativeUpdateItem.Output
{
    public class InitiativeUpdateItemOutput
    {
        public long Id { get; set; }

        public bool DelSign { get; set; }

        public string FileName { get; set; }

        public DateTime? CreditDueTime { get; set; }

        public int Num { get; set; }
    }
}
