using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Service.Record.RecordType.Input
{
    public class RecordTypeUpdateInput
    {
        [Required(ErrorMessage = "档案类型编号是必须的!")]
        public long Id { get; set; }

        [Required(ErrorMessage = "档案类型名称是必须的!")]
        public string RecordTypeName { get; set; }
    }
}
