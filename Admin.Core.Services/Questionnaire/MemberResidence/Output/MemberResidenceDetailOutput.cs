using System;
namespace Admin.Core.Service.Questionnaire.MemberResidence.Output
{
    public class MemberResidenceDetailOutput
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string IdNumber { get; set; }

        public string Address { get; set; }

        public bool LongStay { get; set; }

        public string Relation { get; set; }
    }
}
