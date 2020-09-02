using System;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
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

        public LoanUserController(ILoanUserService loanUserService)
        {
            _loanUserService = loanUserService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddLoanUser(LoanUserAddInput input)
        {
            //var input = Newtonsoft.Json.JsonConvert.DeserializeObject<LoanUserAddInput>(obj["addForm"].ToString());
            return await _loanUserService.AddLoanUserAsync(input);
        }
    }
}
