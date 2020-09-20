using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Service.Questionnaire.HouseHold;
using Admin.Core.Service.Questionnaire.SectionCode;
using Admin.Core.Service.Questionnaire.UserPower;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Admin.Core.Controllers.Questionnaire
{
    public class HouseHoldController : AreaController
    {
        private readonly IHouseHoldService _houseHoldService;
        private readonly IUser _user;
        private readonly IUserPowerService _userPowerService;
        private readonly ISectionCodeService _sectionCodeService;

        public HouseHoldController(IHouseHoldService houseHoldService
            , IUser user
            , IUserPowerService userPowerService
            , ISectionCodeService sectionCodeService)
        {
            _user = user;
            _houseHoldService = houseHoldService;
            _userPowerService = userPowerService;
            _sectionCodeService = sectionCodeService;
        }

        /// <summary>
        /// 获取未评价分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Page(PageInput<HouseHoldEntity> input)
        {
            var belongedStreet = await _userPowerService.GetPowerListAsync(_user.Id);
            return await _houseHoldService.PageAsync(input, belongedStreet, _user.Id);
        }

        /// <summary>
        /// 获取计算结果
        /// </summary>
        /// <param name="code">行政村编码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> CalculatePage(string code)
        {
            return await _houseHoldService.CalculatePageAsync(code);
        }

        /// <summary>
        /// 获取行政村代码列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetSectionCodeList()
        {
            return await _sectionCodeService.GetListAsync();
        }
    }
}
