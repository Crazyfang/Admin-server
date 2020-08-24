﻿using System;
using System.Collections.Generic;
using Admin.Core.Service.Record.Record.Output;

namespace Admin.Core.Service.Record.RecordBorrow.Output
{
    public class ReturnPageOutput
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string UserCode { get; set; }

        public string DepartmentName { get; set; }

        public string DepartmentCode { get; set; }

        public int RecordCount { get; set; }

        public DateTime CreatedTime { get; set; }

        public int BorrowType { get; set; }

        public List<ReturnRecordOutput> Children { get; set; }
    }
}
