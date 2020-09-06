using System;
using System.Collections.Generic;
using Admin.Core.Service.Loan.CompressType.Output;

namespace Admin.Core.Service.Loan.CompressDeadline.Output
{
    public class CompressDeadlineInfoOutput
    {
        public long Id { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public List<CompressTypeInfoOutput> Item { get; set; }
    }
}
