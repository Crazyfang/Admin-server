using System;
using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Antimoney
{
    [Table(Name = "am_currency")]
    public class CurrencyEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }

        public string CurrencyName { get; set; }
    }
}
