using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Service.Questionnaire.Appraise;
using Admin.Core.Service.Questionnaire.Appraise.Input;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Admin.Core.Controllers.Questionnaire
{
    public class AppraiseController : AreaController
    {
        private readonly IAppraiseService _appraieService;
        private readonly IUser _user;

        public AppraiseController(IAppraiseService appraiseService
            , IUser user)
        {
            _appraieService = appraiseService;
            _user = user;
        }

        /// <summary>
        /// 获取已评价分页
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Page(PageInput<AppraiseEntity> input)
        {
            return await _appraieService.PageAsync(input, _user.Id);
        }

        /// <summary>
        /// 获取评价详情
        /// </summary>
        /// <param name="id">评级详情主键</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Detail(long id)
        {
            return await _appraieService.DetailAsync(id);
        }

        /// <summary>
        /// 添加评价
        /// </summary>
        /// <param name="input">评价详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(AppraiseAddInput input)
        {
            return await _appraieService.AddAsync(input, _user.Id);
        }

        /// <summary>
        /// 评价列表返回
        /// </summary>
        /// <param name="id">户口表主键</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> AddInfoReturn(string id)
        {
            return await _appraieService.AddInfoReturnAsync(id);
        }
    }
}
