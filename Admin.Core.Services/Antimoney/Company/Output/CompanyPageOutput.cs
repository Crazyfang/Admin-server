using System;
namespace Admin.Core.Service.Antimoney.Company.Output
{
    public class CompanyPageOutput
    {
        public long Id { get; set; }

        public string CompanyName { get; set; }

        public string BusinessLicense { get; set; }

        public bool NoticeSign { get; set; }
    }
}
