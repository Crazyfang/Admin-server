using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Service.Questionnaire.Appraise.Input;

namespace Admin.Core.Service.Questionnaire.Appraise
{
    public interface IAppraiseService
    {
        /// <summary>
        /// 获取评价界面列表
        /// </summary>
        /// <param name="id">户口主键</param>
        /// <returns></returns>
        Task<IResponseOutput> AddInfoReturnAsync(string id);

        /// <summary>
        /// 添加评价
        /// </summary>
        /// <param name="input">评价添加类</param>
        /// <param name="userId">评价人主键</param>
        /// <returns></returns>
        Task<IResponseOutput> AddAsync(AppraiseAddInput input, long userId);

        /// <summary>
        /// 已评价分页
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        Task<IResponseOutput> PageAsync(PageInput<AppraiseEntity> input, long userId);

        /// <summary>
        /// 评价详情返回
        /// </summary>
        /// <param name="id">评价主键</param>
        /// <returns></returns>
        Task<IResponseOutput> DetailAsync(long id);
    }
}
