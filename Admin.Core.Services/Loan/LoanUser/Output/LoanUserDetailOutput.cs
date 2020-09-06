using System;
using System.Collections.Generic;
using Admin.Core.Service.Loan.CompressDeadline.Output;

namespace Admin.Core.Service.Loan.LoanUser.Output
{
    public class LoanUserDetailOutput
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string UserCode { get; set; }

        public DateTime BeginTime { get; set; }

        public List<CompressDeadlineDetailOutput> Budget { get; set; }
    }
}
