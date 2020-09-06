using System;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Loan;
using Admin.Core.Repository.Loan.LoanMethod;
using AutoMapper;

namespace Admin.Core.Service.Loan.LoanMethod
{
    public class LoanMethodService:ILoanMethodService
    {
        private readonly ILoanMethodRepository _loanMethodRepository;
        private readonly IMapper _mapper;
        public LoanMethodService(IMapper mapper
            , ILoanMethodRepository loanMethodRepository)
        {
            _mapper = mapper;
            _loanMethodRepository = loanMethodRepository;
        }

        public async Task<IResponseOutput> AddLoanMethodAsync(LoanMethodEntity input)
        {
            input.OverTime = input.BeginTime.AddDays(input.CountDay);
            await _loanMethodRepository.InsertAsync(input);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> EditLoanMethodAsync(LoanMethodEntity input)
        {
            input.OverTime = input.BeginTime.AddDays(input.CountDay);
            await _loanMethodRepository.UpdateAsync(input);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> DelLoanMethodAsync(long id)
        {
            await _loanMethodRepository.DeleteAsync(id);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetLoanMethodAsync(long id)
        {
            var entity = await _loanMethodRepository.Select
                .WhereDynamic(id)
                .ToOneAsync();

            return ResponseOutput.Ok(entity);
        }

        public async Task<IResponseOutput> GetLoanMethodPageAsync(PageInput<LoanMethodEntity> input)
        {
            var entityList = await _loanMethodRepository.Select
                .WhereIf(!string.IsNullOrEmpty(input.Filter.UserCode), i => i.UserCode == input.Filter.UserCode)
                .WhereIf(!string.IsNullOrEmpty(input.Filter.UserName), i => i.UserCode == input.Filter.UserName)
                .Include(i => i.VerifyUser)
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var data = new PageOutput<LoanMethodEntity>()
            {
                Total = total,
                List = entityList
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> VerifyLoanMethodAsync(long id, long userId)
        {
            var entity = await _loanMethodRepository.Select
                .WhereDynamic(id)
                .ToOneAsync();

            entity.VerifyUserId = userId;
            entity.OverSign = 2;
            entity.VerifyTime = DateTime.Now;

            await _loanMethodRepository.UpdateAsync(entity);

            return ResponseOutput.Ok();
        }
    }
}
