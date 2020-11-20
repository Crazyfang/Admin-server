using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Repository.Antimoney.Company;
using Admin.Core.Repository.Antimoney.Contract;
using Admin.Core.Repository.Antimoney.File;
using Admin.Core.Service.Antimoney.Company.Input;
using Admin.Core.Service.Antimoney.Company.Output;
using AutoMapper;

namespace Admin.Core.Service.Antimoney.Company
{
    public class CompanyService:ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepositry;
        private readonly IContractRepository _contractRepository;
        private readonly IFileRepository _fileRepository;
        public CompanyService(ICompanyRepository companyRepository
            , IMapper mapper
            , IContractRepository contractRepository
            , IFileRepository fileRepository)
        {
            _mapper = mapper;
            _companyRepositry = companyRepository;
            _contractRepository = contractRepository;
            _fileRepository = fileRepository;
        }

        public async Task<IResponseOutput> CompanyPageAsync(PageInput<CompanyEntity> input)
        {
            var list = await _companyRepositry.Select
                .WhereIf(!string.IsNullOrEmpty(input.Filter.CompanyName), i => i.CompanyName.Contains(input.Filter.CompanyName))
                .IncludeMany(i => i.Contracts)
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var output = new PageOutput<CompanyPageOutput>()
            {
                Total = total,
                List = _mapper.Map<List<CompanyPageOutput>>(list)
            };

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> AddCompanyAsync(CompanyAddInput input)
        {
            var entity = _mapper.Map<CompanyEntity>(input);

            await _companyRepositry.InsertAsync(entity);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> EditCompanyAsync(CompanyEditInput input)
        {
            var entity = await _companyRepositry.GetAsync(input.Id);

            entity = _mapper.Map(input, entity);
            await _companyRepositry.UpdateAsync(entity);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetCompanyAsync(long companyId)
        {
            var entity = await _companyRepositry.GetAsync(companyId);

            var output = _mapper.Map<CompanyInfoOutput>(entity);
            return ResponseOutput.Ok(output);
        }

        [Transaction]
        public async Task<IResponseOutput> DeleteCompanyAsync(long companyId)
        {
            await _companyRepositry.DeleteAsync(i => i.Id == companyId);
            var deletedContractIdList = await _contractRepository.Select.Where(i => i.CompanyId == companyId).ToListAsync(i => i.Id);
            await _fileRepository.DeleteAsync(i => deletedContractIdList.Contains(i.ContractId));
            await _contractRepository.DeleteAsync(i => i.CompanyId == companyId);

            return ResponseOutput.Ok();
        }
    }
}
