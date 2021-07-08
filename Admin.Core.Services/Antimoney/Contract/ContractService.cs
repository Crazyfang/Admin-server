using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Antimoney;
using Admin.Core.Repository.Antimoney.Contract;
using Admin.Core.Repository.Antimoney.File;
using Admin.Core.Repository.Antimoney.PresetFile;
using Admin.Core.Service.Antimoney.Contract.Input;
using Admin.Core.Service.Antimoney.Contract.Output;
using Admin.Core.Service.Antimoney.File.Output;
using AutoMapper;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Admin.Core.Service.Antimoney.Contract
{
    public class ContractService:IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IPresetFileRepository _presetFileRepository;
        private readonly IMapper _mapper;
        public ContractService(IMapper mapper
            , IContractRepository contractRepository
            , IFileRepository fileRepository
            , IPresetFileRepository presetFileRepository)
        {
            _fileRepository = fileRepository;
            _contractRepository = contractRepository;
            _presetFileRepository = presetFileRepository;
            _mapper = mapper;
        }

        public async Task<IResponseOutput> ContractPageAsync(PageInput<ContractSearchInput> input)
        {
            var list = await _contractRepository.Select
                .Where(i => i.SettlementDate >= DateTime.Parse("2020-6-1"))
                .WhereIf(input.Filter.CompanyId != 0, i => i.CompanyId == input.Filter.CompanyId)
                .WhereIf(!string.IsNullOrEmpty(input.Filter.ContractNo), i => i.ContractNo.Contains(input.Filter.ContractNo))
                .WhereIf(input.Filter.Amount.HasValue, i => i.Amount == input.Filter.Amount)
                .IncludeMany(i => i.Files)
                .Include(i => i.Currency)
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var output = new PageOutput<ContractPageOutput>()
            {
                Total = total,
                List = _mapper.Map<List<ContractPageOutput>>(list)
            };

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> AddContractAsync(ContractAddInput input)
        {
            var entity = _mapper.Map<ContractEntity>(input);
            await _contractRepository.InsertAsync(entity);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> EditContractAsync(ContractEditInput input)
        {
            var entity = await _contractRepository.GetAsync(input.Id);
            if(entity == null)
            {
                return ResponseOutput.NotOk("找不到该条记录!");
            }
            else
            {
                entity = _mapper.Map(input, entity);
                await _contractRepository.UpdateAsync(entity);

                return ResponseOutput.Ok();
            }
        }

        public async Task<IResponseOutput> ReturnFileByContractIdAsync(long contractId)
        {
            var entitys = await _fileRepository.Select
                .Where(i => i.ContractId == contractId)
                .IncludeMany(i => i.PictureList)
                .ToListAsync();

            if(entitys.Count == 0)
            {
                var list = await _presetFileRepository.Select.ToListAsync();
                var results = _mapper.Map<List<FileAddOutput>>(list);

                foreach(var item in results)
                {
                    item.ContractId = contractId;
                }

                return ResponseOutput.Ok(results);
            }
            else
            {
                var results = _mapper.Map<List<FileAddOutput>>(entitys);

                return ResponseOutput.Ok(results);
            }
        }

        public async Task<IResponseOutput> GetContractAsync(long contractId)
        {
            var entity = await _contractRepository.GetAsync(contractId);

            var output = _mapper.Map<ContractEditInput>(entity);

            return ResponseOutput.Ok(output);
        }

        [Transaction]
        public async Task<IResponseOutput> DeleteContractAsync(long contratId)
        {
            await _contractRepository.DeleteAsync(i => i.Id == contratId);
            await _fileRepository.DeleteAsync(i => i.ContractId == contratId);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetNoticeAsync(long contractId)
        {
            var entity = await _contractRepository.GetAsync(contractId);

            var output = _mapper.Map<ContractNoticeInput>(entity);

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> AddOrEditNoticeAsync(ContractNoticeInput input)
        {
            var entity = await _contractRepository.GetAsync(input.ContractId);

            entity.AwakeTime = input.AwakeTime;
            entity.AwakeNotes = input.AwakeNotes;
            entity.EnableSign = input.EnableSign;

            await _contractRepository.UpdateAsync(entity);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GenerateContractListAsync(PageInput<ContractSearchInput> input)
        {
            var list = await _contractRepository.Select
                .WhereIf(input.Filter.CompanyId != 0, i => i.CompanyId == input.Filter.CompanyId)
                .WhereIf(!string.IsNullOrEmpty(input.Filter.ContractNo), i => i.ContractNo == input.Filter.ContractNo)
                .Include(i => i.Currency)
                .ToListAsync();

            var fileName = Guid.NewGuid() + ".xlsx";
            string fileSrc = Path.Combine(AppContext.BaseDirectory, "Files", fileName);
            var rowIndex = 1;

            FileStream stream = new FileStream(fileSrc,
                                              FileMode.Create,
                                              FileAccess.Write);
            IWorkbook wb = new XSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Sheet1");

            var row = sheet.CreateRow(0);

            // 提单查询日期
            var col = row.CreateCell(0);
            col.SetCellValue("提单查询日期");

            // 合同号
            col = row.CreateCell(1);
            col.SetCellValue("合同号");

            // 汇款人姓名
            col = row.CreateCell(2);
            col.SetCellValue("汇款人名称");

            // 汇款人地址
            col = row.CreateCell(3);
            col.SetCellValue("汇款人地址");

            // 收货人姓名
            col = row.CreateCell(4);
            col.SetCellValue("收货人名称");

            // 收货人地址
            col = row.CreateCell(5);
            col.SetCellValue("收货人地址");

            // 币种
            col = row.CreateCell(6);
            col.SetCellValue("币种");

            // 金额
            col = row.CreateCell(7);
            col.SetCellValue("金额");

            // 解付日期
            col = row.CreateCell(8);
            col.SetCellValue("解付日期");

            // 款项性质
            col = row.CreateCell(9);
            col.SetCellValue("款项性质");

            // 发货日期
            col = row.CreateCell(10);
            col.SetCellValue("发货日期");

            // 资料提交情况
            col = row.CreateCell(11);
            col.SetCellValue("资料提交情况");

            // 备注
            col = row.CreateCell(12);
            col.SetCellValue("备注");

            foreach(var item in list)
            {
                row = sheet.CreateRow(rowIndex);

                col = row.CreateCell(0);
                col.SetCellValue(item.LadingInquireDate.HasValue ? item.LadingInquireDate.Value.ToString("yyyy-MM-dd") : "");

                col = row.CreateCell(1);
                col.SetCellValue(item.ContractNo);

                col = row.CreateCell(2);
                col.SetCellValue(item.RemitterName);

                col = row.CreateCell(3);
                col.SetCellValue(item.RemitterAddress);

                col = row.CreateCell(4);
                col.SetCellValue(item.ConsigneeName);

                col = row.CreateCell(5);
                col.SetCellValue(item.ConsigneeAddress);

                col = row.CreateCell(6);
                col.SetCellValue(item.Currency.CurrencyName);

                col = row.CreateCell(7);
                col.SetCellValue(item.Amount);

                col = row.CreateCell(8);
                col.SetCellValue(item.SettlementDate.ToString("yyyy-MM-dd"));

                col = row.CreateCell(9);
                col.SetCellValue(item.PaymentNature);

                col = row.CreateCell(10);
                col.SetCellValue(item.DeliveryDate.ToString("yyyy-MM-dd"));

                col = row.CreateCell(11);
                col.SetCellValue(item.DataSubmitInfo);

                col = row.CreateCell(12);
                col.SetCellValue(item.Remarks);

                rowIndex += 1;
            }
            wb.Write(stream);
            wb.Close();
            stream.Close();

            return ResponseOutput.Ok(fileName);
        }
    }
}
