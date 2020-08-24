using System;
using System.Collections.Generic;

namespace Admin.Core.Service.Record.RecordBorrow.Input
{
    public class RecordBorrowAddInput
    {
        public int BorrowType { get; set; }

        public List<long> RecordBorrowItemIdList { get; set; }

        public int ReturnSign { get; set; }
    }
}
