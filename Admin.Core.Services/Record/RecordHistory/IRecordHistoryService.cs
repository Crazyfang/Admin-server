using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;

namespace Admin.Core.Service.Record.RecordHistory
{
    public partial interface IRecordHistoryService
    {
        Task<IResponseOutput> GetAsync(long id);
    }
}
