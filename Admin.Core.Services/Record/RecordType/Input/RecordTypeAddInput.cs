using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Service.Record.RecordType.Input
{
    public class RecordTypeAddInput
    {
        /// <summary>
        /// 档案类型名字
        /// </summary>
        [Required(ErrorMessage = "档案类型名称是必须的!")]
        public string RecordTypeName { get; set; }
    }
}
