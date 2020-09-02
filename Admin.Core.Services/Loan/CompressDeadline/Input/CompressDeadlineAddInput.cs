using System;
using System.Collections.Generic;
using Admin.Core.Service.Loan.CompressType.Input;

namespace Admin.Core.Service.Loan.CompressDeadline.Input
{
    public class CompressDeadlineAddInput
    {
        public int CountMonth { get; set; }

        public List<CompressTypeAddInput> Item { get; set; }
    }
}
