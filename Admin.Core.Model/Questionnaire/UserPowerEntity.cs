using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Questionnaire
{
    [Table(Name = "qn_userpower")]
    public class UserPowerEntity
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Power { get; set; }
    }
}
