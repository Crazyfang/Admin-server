using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Attributes;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Service.Questionnaire.HouseHold;
using Admin.Core.Service.Questionnaire.SectionCode;
using Admin.Core.Service.Questionnaire.UserPower;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
            return await _houseHoldService.CalculatePageAsync(code, _user.Id);
        }

        /// <summary>
        /// 获取行政村代码列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetSectionCodeList()
        {
            return await _sectionCodeService.GetListAsync(_user.Id);
        }

        /// <summary>
        /// 生成评价明细表
        /// </summary>
        /// <param name="obj">行政村代码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GenerateDetailCollectXlsx(JObject obj)
        {
            var code = obj["code"].ToString();
            var id = long.Parse(obj["id"].ToString());
            return await _houseHoldService.DetailExcelAsync(code, id);
        }

        /// <summary>
        /// 评价明细汇总Excel下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public FileResult DownloadFile(string fileName)
        {
            var stream = System.IO.File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Files", fileName));
            //var stream = System.IO.File.OpenRead(Path.Combine(AppContext.BaseDirectory, "Files", fileName));
            return File(stream, "application/octet-stream", "评价汇总.xlsx");
        }

        /// <summary>
        /// 授信明细表生成
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GenerateCreditDetailXlsx(string code)
        {
            return await _houseHoldService.ResultCollectAsync(code);
        }

        /// <summary>
        /// 授信明细Excel下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public FileResult DownloadCreditCollectFile(string fileName)
        {
            var stream = System.IO.File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Files", fileName));

            return File(stream, "application/octet-stream", "评分明细.xlsx");
        }

        /// <summary>
        /// 获取行政村下的公议人列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetUserPowerSelect(string code)
        {
            return await _houseHoldService.UserPowerSelectAsync(code);
        }
    }
}
