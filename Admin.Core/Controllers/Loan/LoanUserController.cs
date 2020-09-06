using System;
using System.Threading.Tasks;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.LoanUser;
using Admin.Core.Service.Loan.LoanUser.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Admin.Core.Controllers.Loan
{
    public class LoanUserController:AreaController
    {
        private readonly ILoanUserService _loanUserService;
        private readonly IUser _user;

        public LoanUserController(ILoanUserService loanUserService
            , IUser user)
        {
            _loanUserService = loanUserService;
            _user = user;
        }

        /// <summary>
        /// 添加贷款压缩
        /// </summary>
        /// <param name="input">贷款压缩添加信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddLoanUser(LoanUserAddInput input)
        {
            //var input = Newtonsoft.Json.JsonConvert.DeserializeObject<LoanUserAddInput>(obj["addForm"].ToString());
            return await _loanUserService.AddLoanUserAsync(input);
        }

        /// <summary>
        /// 获取贷款压缩基本信息
        /// </summary>
        /// <param name="id">string的id</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            //var id = Newtonsoft.Json.JsonConvert.DeserializeObject<long>(obj["id"].ToString());

            return await _loanUserService.GetLoanUserInfoAsync(id);
        }

        /// <summary>
        /// 编辑贷款压缩
        /// </summary>
        /// <param name="input">贷款压缩编辑信息</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Edit(LoanUserEditInput input)
        {
            return await _loanUserService.EditLoanUserAsync(input);
        }

        /// <summary>
        /// 删除贷款压缩
        /// </summary>
        /// <param name="id">贷款压缩主键</param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _loanUserService.DeleteLoanUserAsync(id);
        }

        /// <summary>
        /// 获取分页信息
        /// </summary>
        /// <param name="input">筛选条件，分页信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetList(PageInput<LoanUserEntity> input)
        {
            return await _loanUserService.GetPageAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Verify(long id)
        {
            return await _loanUserService.VerifyLoanUserAsync(id, _user.Id);
        }
    }

}
