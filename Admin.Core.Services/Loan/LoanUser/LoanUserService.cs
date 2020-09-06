using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Loan;
using Admin.Core.Repository.Loan.CompressDeadline;
using Admin.Core.Repository.Loan.CompressType;
using Admin.Core.Repository.Loan.LoanUser;
using Admin.Core.Service.Loan.LoanUser.Input;
using Admin.Core.Service.Loan.LoanUser.Output;
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
            , IFreeSql freeSql
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

        public async Task<IResponseOutput> GetLoanUserInfoAsync(long id)
        {
            var entity = await _loanUserRepository.Select
                .WhereDynamic(id)
                .IncludeMany(i => i.Budget, then => then.IncludeMany(j => j.Item))
                .ToOneAsync();

            var output = _mapper.Map<LoanUserDetailOutput>(entity);

            return ResponseOutput.Ok(output);
        }

        [Transaction]
        public async Task<IResponseOutput> EditLoanUserAsync(LoanUserEditInput input)
        {
            var entity = _mapper.Map<LoanUserEntity>(input);

            await _loanUserRepository.UpdateAsync(entity);
            var compressTypeIdList = await _compressTypeRepository.Select.Where(i => i.LoadUserId == input.Id).ToListAsync(i => i.Id);
            foreach(var item in compressTypeIdList)
            {
                await _compressTypeRepository.DeleteAsync(item);
            }
            var compressDeadLineIdList = await _compressDeadlineRepository.Select.Where(i => i.UserId == input.Id).ToListAsync(i => i.Id);
            foreach (var item in compressTypeIdList)
            {
                await _compressDeadlineRepository.DeleteAsync(item);
            }

            foreach (var item in entity.Budget)
            {
                item.BeginTime = input.BeginTime;
                item.EndTime = input.BeginTime.AddMonths(item.CountMonth);
                item.UserId = entity.Id;

                var compressDeadLine = await _compressDeadlineRepository.InsertAsync(item);

                foreach (var compressType in item.Item.Where(i => i.Checked == true))
                {
                    compressType.CompressDeadlineId = compressDeadLine.Id;
                    compressType.LoadUserId = entity.Id;

                    await _compressTypeRepository.InsertAsync(compressType);
                }
            }

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetPageAsync(PageInput<LoanUserEntity> input)
        {
            var entityList = await _loanUserRepository.Select
                .WhereIf(!string.IsNullOrEmpty(input.Filter.UserCode), i => i.UserCode == input.Filter.UserCode)
                .WhereIf(!string.IsNullOrEmpty(input.Filter.UserName), i => i.UserCode == input.Filter.UserName)
                .Include(i => i.VerifyUser)
                .IncludeMany(i => i.Budget, then => then.IncludeMany(j => j.Item))
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var data = new PageOutput<LoanUserInfoOutput>()
            {
                List = _mapper.Map<List<LoanUserInfoOutput>>(entityList),
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        [Transaction]
        public async Task<IResponseOutput> DeleteLoanUserAsync(long id)
        {
            await _loanUserRepository.DeleteAsync(id);
            var compressTypeIdList = await _compressTypeRepository.Select.Where(i => i.LoadUserId == id).ToListAsync(i => i.Id);
            foreach (var item in compressTypeIdList)
            {
                await _compressTypeRepository.DeleteAsync(item);
            }
            var compressDeadLineIdList = await _compressDeadlineRepository.Select.Where(i => i.UserId == id).ToListAsync(i => i.Id);
            foreach (var item in compressTypeIdList)
            {
                await _compressDeadlineRepository.DeleteAsync(item);
            }

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> VerifyLoanUserAsync(long id, long userId)
        {
            var entity = await _loanUserRepository.Select
                .WhereDynamic(id)
                .ToOneAsync();

            entity.VerifyUserId = userId;
            entity.OverSign = 2;
            entity.VerifyTime = DateTime.Now;

            await _loanUserRepository.UpdateAsync(entity);

            return ResponseOutput.Ok();
        }
    }
}
