using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Repository.Record.Notify;

namespace Admin.Core.Service.Record.Notify
{
    public class NotifyService:INotifyService
    {
        private readonly INotifyRepository _notifyRepository;

        public NotifyService(INotifyRepository notifyRepository)
        {
            _notifyRepository = notifyRepository;
        }

        public async Task<IResponseOutput> GetPageAsync(PageInput<NotifyEntity> input, long userId)
        {
            var res = await _notifyRepository.Select
                .Where(i => i.UserId == userId)
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .OrderBy(i => i.Sign)
                .OrderByDescending(i => i.Id)
                .ToListAsync();

            var data = new PageOutput<NotifyEntity>()
            {
                List = res,
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> ReadAsync(long id)
        {
            var res = await _notifyRepository.UpdateDiy.Set(i => i.Sign, true).WhereDynamic(id).ExecuteAffrowsAsync();

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetCountAsync(long id)
        {
            var res = await _notifyRepository.Select
                .Where(i => i.UserId == id && i.Sign == false)
                .CountAsync();

            return ResponseOutput.Ok(res);
        }

        public async Task<IResponseOutput> ReadNotifyAsync(long id)
        {
            var res = await _notifyRepository.UpdateDiy.Set(i => i.Sign, true).Set(i => i.ReadTime, DateTime.Now).WhereDynamic(id).ExecuteAffrowsAsync();

            if(res > 0)
            {
                return ResponseOutput.Ok();
            }
            else
            {
                return ResponseOutput.NotOk();
            }
        }

        public async Task<IResponseOutput> InsertAsync(long id, string reason)
        {
            var entity = new NotifyEntity()
            {
                UserId = id,
                Message = reason
            };

            await _notifyRepository.InsertAsync(entity);

            return ResponseOutput.Ok();
        }
    }
}
