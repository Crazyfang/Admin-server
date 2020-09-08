using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;

namespace Admin.Core.Service.Record.Notify
{
    public partial interface INotifyService
    {
        /// <summary>
        /// 指定用户通知分页
        /// </summary>
        /// <param name="input">分页参数及筛选条件</param>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageAsync(PageInput<NotifyEntity> input, long userId);

        /// <summary>
        /// 获取未读通知总数
        /// </summary>
        /// <param name="id">用户主键</param>
        /// <returns></returns>
        Task<IResponseOutput> GetCountAsync(long id);

        /// <summary>
        /// 通知已读
        /// </summary>
        /// <param name="id">通知主键</param>
        /// <returns></returns>
        Task<IResponseOutput> ReadNotifyAsync(long id);

        /// <summary>
        /// 异步插入未读通知
        /// </summary>
        /// <param name="id">用户主键</param>
        /// <param name="reason">通知消息</param>
        /// <returns></returns>
        Task<IResponseOutput> InsertAsync(long id, string reason);
    }
}
