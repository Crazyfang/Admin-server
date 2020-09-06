using System;
using System.Threading.Tasks;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Loan;
using Admin.Core.Service.Loan.LoanMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Core.Controllers.Loan
{
    public class LoanMethodChangeController:AreaController
    {
        private readonly ILoanMethodService _loanMethodService;
        private readonly IUser _user;
        public LoanMethodChangeController(ILoanMethodService loanMethodService
            , IUser user)
        {
            _loanMethodService = loanMethodService;
            _user = user;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _loanMethodService.GetLoanMethodAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(LoanMethodEntity input)
        {
            return await _loanMethodService.AddLoanMethodAsync(input);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Edit(LoanMethodEntity input)
        {
            return await _loanMethodService.EditLoanMethodAsync(input);
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _loanMethodService.DelLoanMethodAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPage(PageInput<LoanMethodEntity> input)
        {
            return await _loanMethodService.GetLoanMethodPageAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> VerifyLoanMethod(long id)
        {
            return await _loanMethodService.VerifyLoanMethodAsync(id, _user.Id);
        }
    }
}
