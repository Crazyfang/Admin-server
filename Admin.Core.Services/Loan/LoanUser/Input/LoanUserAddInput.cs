using System;
using System.Collections.Generic;
using Admin.Core.Service.Loan.CompressDeadline.Input;

namespace Admin.Core.Service.Loan.LoanUser.Input
{
    public class LoanUserAddInput
    {
        public string UserName { get; set; }

        public string UserCode { get; set; }

        public DateTime BeginTime { get; set; }

        //public int TimeType { get; set; }

        public List<CompressDeadlineAddInput> Budget { get; set; }
    }
}
