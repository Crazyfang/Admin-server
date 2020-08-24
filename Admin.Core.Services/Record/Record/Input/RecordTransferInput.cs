using System;
using System.Collections.Generic;

namespace Admin.Core.Service.Record.Record.Input
{
    public class RecordTransferInput
    {
        public string Id { get; set; }

        public long Value { get; set; }

        public List<long> SelectRecord { get; set; }
    }
}
