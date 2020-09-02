using System;
namespace Admin.Core.Service.Loan.CompressType.Input
{
    public class CompressTypeAddInput
    {
        public string Name { get; set; }

        public bool Checked { get; set; }

        public int? WantedValue { get; set; }
    }
}
