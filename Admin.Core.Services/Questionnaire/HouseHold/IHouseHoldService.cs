using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Questionnaire;

namespace Admin.Core.Service.Questionnaire.HouseHold
{
    public interface IHouseHoldService
    {
        /// <summary>
        /// 未答公议分页
        /// </summary>
        /// <param name="input">分页信息</param>
        /// <param name="belongedStreet">所属街道列表</param>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        Task<IResponseOutput> PageAsync(PageInput<HouseHoldEntity> input, List<string> belongedStreet, long userId);

        /// <summary>
        /// 获取计算结果
        /// </summary>
        /// <param name="code">行政村代码</param>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        Task<IResponseOutput> CalculatePageAsync(string code, long userId);

        /// <summary>
        /// 用户评价明细数据表导出
        /// </summary>
        /// <param name="code">行政村代码</param>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DetailExcelAsync(string code, long userId);

        /// <summary>
        /// 授信明细导出
        /// </summary>
        /// <param name="code">行政村代码</param>
        /// <returns></returns>
        Task<IResponseOutput> ResultCollectAsync(string code);

        /// <summary>
        /// 获取行政村公议用户
        /// </summary>
        /// <param name="code">行政村代码</param>
        /// <returns></returns>
        Task<IResponseOutput> UserPowerSelectAsync(string code);
    }
}
