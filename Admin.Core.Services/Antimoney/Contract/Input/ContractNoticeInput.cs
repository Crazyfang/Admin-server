using System;
namespace Admin.Core.Service.Antimoney.Contract.Input
{
    public class ContractNoticeInput
    {
        public long ContractId { get; set; }

        public DateTime? AwakeTime { get; set; }

        public string AwakeNotes { get; set; }

        public bool EnableSign { get; set; }
    }
}
