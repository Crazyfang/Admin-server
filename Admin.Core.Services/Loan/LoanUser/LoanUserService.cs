using System;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Output;
using Admin.Core.Model.Loan;
using Admin.Core.Repository.Loan.CompressDeadline;
using Admin.Core.Repository.Loan.CompressType;
using Admin.Core.Repository.Loan.LoanUser;
using Admin.Core.Service.Loan.LoanUser.Input;
using AutoMapper;

namespace Admin.Core.Service.Loan.LoanUser
{
    public class LoanUserService:ILoanUserService
    {
        private readonly ILoanUserRepository _loanUserRepository;
        private readonly ICompressTypeRepository _compressTypeRepository;
        private readonly ICompressDeadlineRepository _compressDeadlineRepository;
        private readonly IMapper _mapper;

        public LoanUserService(ILoanUserRepository loanUserRepository
            , ICompressTypeRepository compressTypeRepository
            , ICompressDeadlineRepository compressDeadlineRepository
            , IMapper mapper)
        {
            _mapper = mapper;
            _loanUserRepository = loanUserRepository;
            _compressDeadlineRepository = compressDeadlineRepository;
            _compressTypeRepository = compressTypeRepository;
        }

        [Transaction]
        public async Task<IResponseOutput> AddLoanUserAsync(LoanUserAddInput input)
        {
            var loanUserEntity = _mapper.Map<LoanUserEntity>(input);
            var entity = await _loanUserRepository.InsertAsync(loanUserEntity);

            foreach(var item in loanUserEntity.Budget)
            {
                item.BeginTime = input.BeginTime;
                item.EndTime = input.BeginTime.AddMonths(item.CountMonth);
                item.UserId = entity.Id;

                var compressDeadLine = await _compressDeadlineRepository.InsertAsync(item);

                foreach(var compressType in item.Item.Where(i => i.Checked == true))
                {
                    compressType.CompressDeadlineId = compressDeadLine.Id;
                    compressType.LoadUserId = entity.Id;

                    await _compressTypeRepository.InsertAsync(compressType);
                }
            }

            return ResponseOutput.Ok();
        }
    }
}
