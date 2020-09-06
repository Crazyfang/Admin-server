using System;
using System.Collections.Generic;
using Admin.Core.Service.Loan.CompressType.Output;

namespace Admin.Core.Service.Loan.CompressDeadline.Output
{
    public class CompressDeadlineDetailOutput
    {
        public int CountMonth { get; set; }

        public List<CompressTypeDetailOutput> Item { get; set; }
    }
}
