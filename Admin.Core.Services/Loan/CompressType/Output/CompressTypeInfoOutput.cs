using System;
namespace Admin.Core.Service.Loan.CompressType.Output
{
    public class CompressTypeInfoOutput
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public double? WantedValue { get; set; }

        public double? PresentValue { get; set; }
    }
}
