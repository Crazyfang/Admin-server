using System;
using System.IO;
using System.Threading.Tasks;
using Admin.Core.Attributes;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Service.Antimoney.Contract;
using Admin.Core.Service.Antimoney.Contract.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Antimoney
{
    public class ContractController:AreaController
    {
        private readonly IContractService _contractService;
        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        /// <summary>
        /// 合同分页
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Page(PageInput<ContractSearchInput> input)
        {
            return await _contractService.ContractPageAsync(input);
        }

        /// <summary>
        /// 添加合同
        /// </summary>
        /// <param name="input">合同基本信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(ContractAddInput input)
        {
            return await _contractService.AddContractAsync(input);
        }

        /// <summary>
        /// 编辑合同
        /// </summary>
        /// <param name="input">合同基本信息</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Edit(ContractEditInput input)
        {
            return await _contractService.EditContractAsync(input);
        }

        /// <summary>
        /// 根据合同号返回文件列表
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetFileList(long contractId)
        {
            return await _contractService.ReturnFileByContractIdAsync(contractId);
        }

        /// <summary>
        /// 根据合同主键获取合同信息
        /// </summary>
        /// <param name="contractId">合同主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long contractId)
        {
            return await _contractService.GetContractAsync(contractId);
        }

        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="contractId">合同主键</param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long contractId)
        {
            return await _contractService.DeleteContractAsync(contractId);
        }

        /// <summary>
        /// 获取提醒
        /// </summary>
        /// <param name="contractId">合同主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetNotes(long contractId)
        {
            return await _contractService.GetNoticeAsync(contractId);
        }

        /// <summary>
        /// 更新提醒
        /// </summary>
        /// <param name="input">提醒内容</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddOrEditNotes(ContractNoticeInput input)
        {
            return await _contractService.AddOrEditNoticeAsync(input);
        }

        /// <summary>
        /// 生成合同Excel文件
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns>文件地址</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GenerateContratList(PageInput<ContractSearchInput> input)
        {
            return await _contractService.GenerateContractListAsync(input);
        }

        /// <summary>
        /// 下载合同Excel文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public FileResult DownloadFile(string fileName)
        {
            var stream = System.IO.File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Files", fileName));
            //var stream = System.IO.File.OpenRead(Path.Combine(AppContext.BaseDirectory, "Files", fileName));
            return File(stream, "application/octet-stream", "导出合同.xlsx");
        }
    }
}
