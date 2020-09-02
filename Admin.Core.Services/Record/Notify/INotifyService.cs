using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;

namespace Admin.Core.Service.Record.Notify
{
    public partial interface INotifyService
    {
        Task<IResponseOutput> GetPageAsync(PageInput<NotifyEntity> input, long userId);

        Task<IResponseOutput> ReadAsync(long id);

        Task<IResponseOutput> GetCountAsync(long id);

        Task<IResponseOutput> ReadNotifyAsync(long id);

        Task<IResponseOutput> InsertAsync(long id, string reason);
    }
}
