using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.Company;
using Admin.Core.Service.Antimoney.Company.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Antimoney
{
    public class CompanyController:AreaController
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// 获取分页公司信息
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IResponseOutput> Page(PageInput<CompanyEntity> input)
        {
            return await _companyService.CompanyPageAsync(input);
        }

        /// <summary>
        /// 添加公司信息
        /// </summary>
        /// <param name="input">公司基本信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(CompanyAddInput input)
        {
            return await _companyService.AddCompanyAsync(input);
        }

        /// <summary>
        /// 编辑公司信息
        /// </summary>
        /// <param name="input">公司信息</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Edit(CompanyEditInput input)
        {
            return await _companyService.EditCompanyAsync(input);
        }

        /// <summary>
        /// 根据公司主键获取公司信息
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long companyId)
        {
            return await _companyService.GetCompanyAsync(companyId);
        }

        /// <summary>
        /// 删除公司
        /// </summary>
        /// <param name="companyId">公司主键</param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long companyId)
        {
            return await _companyService.DeleteCompanyAsync(companyId);
        }
    }
}
