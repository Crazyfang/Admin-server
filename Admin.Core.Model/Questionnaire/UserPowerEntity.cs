using System;
using Admin.Core.Model.Admin;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Questionnaire
{
    [Table(Name = "qn_userpower")]
    public class UserPowerEntity
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        public long UserId { get; set; }

        public UserEntity User { get; set; }

        public string Power { get; set; }
    }
}
