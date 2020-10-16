using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Repository.Questionnaire.Appraise;
using Admin.Core.Repository.Questionnaire.Calculate;
using Admin.Core.Repository.Questionnaire.HouseHold;
using Admin.Core.Repository.Questionnaire.UserPower;
using Admin.Core.Service.Questionnaire.HouseHold.Output;
using AutoMapper;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Admin.Core.Service.Questionnaire.HouseHold
{
    public class HouseHoldService:IHouseHoldService
    {
        private readonly IHouseHoldRepository _houseHoldRepository;
        private readonly IMapper _mapper;
        private readonly ICalculateRepository _calculateRepository;
        private readonly IAppraiseRepository _appraiseRepository;
        private readonly IUserPowerRepository _userPowerRepository;

        public HouseHoldService(IMapper mapper
            , IHouseHoldRepository houseHoldRepository
            , ICalculateRepository calculateRepository
            , IAppraiseRepository appraiseRepository
            , IUserPowerRepository userPowerRepository)
        {
            _mapper = mapper;
            _houseHoldRepository = houseHoldRepository;
            _calculateRepository = calculateRepository;
            _appraiseRepository = appraiseRepository;
            _userPowerRepository = userPowerRepository;
        }

        public async Task<IResponseOutput> PageAsync(PageInput<HouseHoldEntity> input, List<string> belongedStreet, long userId)
        {
            var list = await _houseHoldRepository.Select
            .Where(i => !i.Appraises.AsSelect().Any(i => i.AppraiserId == userId))
            .Where(i => belongedStreet.Contains(i.BelongedStreet))
            .WhereIf(!string.IsNullOrEmpty(input.Filter.HeadUserName), i => i.HeadUserName.Contains(input.Filter.HeadUserName))
            .WhereIf(!string.IsNullOrEmpty(input.Filter.HeadUserIdNumber), i => i.HeadUserIdNumber.Contains(input.Filter.HeadUserIdNumber))
            .Count(out var total)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<HouseHoldPageOutput>()
            {
                List = _mapper.Map<List<HouseHoldPageOutput>>(list),
                Total = total
            };

            return ResponseOutput.Ok(data);
            
            
        }

        public async Task<IResponseOutput> CalculatePageAsync(string code, long userId)
        {
            //if(code == "TYC")
            //{
            //    var count = await _calculateRepository.Select.CountAsync();
            //    if(count == 0)
            //    {
            //        var houseHoldIdList = await _houseHoldRepository.Select.Where(i => i.BelongedStreet == code).ToListAsync(i => i.Id);
            //        foreach(var id in houseHoldIdList)
            //        {
            //            // 建议授信额度测算
            //            float basicQuota = 0.00F;
            //            float familyNetIncome = 0.00F;
            //            float familyNetWorthy = 0.00F;
            //            var Coefficient = 70;
            //            var dangerCount = 0;
            //            var quotaList = new List<float>();
            //            // 承包亩数
            //            var houseHoldEntity = await _houseHoldRepository.Select
            //                .Where(i => i.Id == id)
            //                .ToOneAsync();
            //            var acres = houseHoldEntity.Acres.HasValue ? houseHoldEntity.Acres.Value : 0;

            //            var appraiseList = await _appraiseRepository.Select
            //                .Where(i => i.HouseHoldId == id)
            //                .ToListAsync();
            //            if(appraiseList.Count == 5)
            //            {
            //                foreach (var item in appraiseList)
            //                {
            //                    basicQuota = 0.00F;
            //                    Coefficient = 70;
            //                    familyNetWorthy = 0.00F;

            //                    if (item.Instability || item.Repudiate || item.Reputation || item.Lending || item.Gamble)
            //                    {
            //                        dangerCount += 1;
            //                        continue;
            //                    }

            //                    if (dangerCount >= 2)
            //                    {
            //                        break;
            //                    }

            //                    // 承包土地价值计算
            //                    familyNetWorthy += (float)(acres * 0.72);

            //                    // 自有房产价值计算
            //                    if (item.SelfBuilding)
            //                    {
            //                        if (item.Bungalow)
            //                        {
            //                            familyNetWorthy += item.BungalowCount * 20;
            //                        }
            //                        if (item.Building)
            //                        {
            //                            familyNetWorthy += item.BuildingCount * 40;
            //                        }
            //                        if (item.Cottage)
            //                        {
            //                            familyNetWorthy += item.CottageCount * 60;
            //                        }
            //                    }
            //                    // 商品房价值计算
            //                    if (item.GoodsBuilding)
            //                    {
            //                        familyNetWorthy += item.GoodsBuildingCount * 60;
            //                    }
            //                    // 车辆价值计算
            //                    if (item.CarHold)
            //                    {
            //                        familyNetWorthy += item.CarHoldCount * 5;
            //                    }

            //                    // 家庭净资产计算
            //                    if (item.DebtCondition.HasValue)
            //                    {
            //                        familyNetWorthy -= item.DebtCondition.Value;
            //                    }

            //                    // 家庭年净收入
            //                    if (item.HomeEarning.HasValue && item.HomePay.HasValue)
            //                    {
            //                        familyNetIncome = item.HomeEarning.Value - item.HomePay.Value;
            //                    }

            //                    // 取高值
            //                    basicQuota = (float)(familyNetIncome * 1.5 < familyNetWorthy * 0.3 ? familyNetWorthy * 0.3 : familyNetIncome * 1.5);

            //                    // 负值判断
            //                    if (basicQuota < 0)
            //                    {
            //                        basicQuota = 0;
            //                    }

            //                    // 计算评价系数
            //                    if (item.Condition1)
            //                    {
            //                        Coefficient -= 20;
            //                    }
            //                    if (item.Condition2)
            //                    {
            //                        Coefficient -= 20;
            //                    }
            //                    if (item.Condition3)
            //                    {
            //                        Coefficient -= 10;
            //                    }
            //                    if (item.Condition4)
            //                    {
            //                        Coefficient -= 10;
            //                    }
            //                    if (item.Condition5)
            //                    {
            //                        Coefficient -= 10;
            //                    }

            //                    // 添加到列表中
            //                    quotaList.Add(basicQuota * Coefficient / 70);
            //                }

            //                if (dangerCount >= 2)
            //                {
            //                    var entity = new CalculateEntity()
            //                    {
            //                        HouseHoldId = id,
            //                        RefuseMark = true
            //                    };
            //                    await _calculateRepository.InsertAsync(entity);
            //                }
            //                else
            //                {
            //                    // 偏离度计算
            //                    //var maxValue = quotaList.Max();
            //                    //var minValue = quotaList.Min();
            //                    var sign = false;
            //                    // 风险情况判定
            //                    var riskSign = false;

            //                    //quotaList.Remove(maxValue);
            //                    //quotaList.Remove(minValue);

            //                    // 五个值求取平均值,求取标准差
            //                    //float total = 0.00F;
            //                    double avg = quotaList.Average();
            //                    double sum = quotaList.Sum(d => Math.Pow(d - avg, 2));
            //                    double ret = Math.Sqrt(sum / quotaList.Count());

            //                    // 计算变异系数
            //                    if (ret / avg > 0.4)
            //                    //if (maxValue - minValue > average * 0.5)
            //                    {
            //                        sign = true;
            //                    }
            //                    if (dangerCount == 1)
            //                    {
            //                        riskSign = true;
            //                    }

            //                    // 授信额度大于30万设置30万封顶
            //                    if (avg > 30)
            //                    {
            //                        avg = 30;
            //                    }
            //                    else
            //                    {
            //                        // 取整,四舍六入
            //                        avg = Math.Floor(avg);
            //                    }

            //                    var entity = new CalculateEntity()
            //                    {
            //                        HouseHoldId = id,
            //                        Average = avg,
            //                        StandardDeviation = ret,
            //                        Deviation = ret / avg,
            //                        VarianceSum = sum,
            //                        DangerUserMark = riskSign,
            //                        DeviationMark = sign
            //                    };
            //                    await _calculateRepository.InsertAsync(entity);
            //                    //float total = 0.00F;
            //                    //quotaList.ForEach(i => total += i);
            //                    //float average = total / quotaList.Count();

            //                }
            //            }
            //        }
            //    }
            //}
            var sectionListStr = await _userPowerRepository.Select
                .Where(i => i.UserId == userId)
                .ToOneAsync(i => i.Power);

            var sectionList = new List<string>(sectionListStr.Split(","));

            if (sectionList.Contains(code))
            {
                var list = await _houseHoldRepository.Select
                .Include(i => i.SuggestCreditor)
                .Where(i => i.BelongedStreet == code)
                .OrderBy(i => i.Id)
                .ToListAsync();

                var output = _mapper.Map<List<HouseHoldCalculateOutput>>(list);

                return ResponseOutput.Ok(output);
            }
            else
            {
                return ResponseOutput.NotOk("当前村镇信息您没有权限查看");
            }
            
        }

        public async Task<IResponseOutput> DetailExcelAsync(string code, long userId)
        {
            var appraiseList = await _appraiseRepository.Select
                .Where(i => i.AppraiserId == userId)
                .Where(i => i.HouseHoldId.Contains(code))
                .Include(i => i.HouseHold)
                .IncludeMany(i => i.Members, then => then.Include(h => h.HouseHoldMember))
                .ToListAsync();

            string baseFileUrl = Path.Combine(AppContext.BaseDirectory, "Files", "detailcollect.xlsx");
            string targetFileName = Guid.NewGuid() + ".xlsx";
            string targetFileUrl = Path.Combine(AppContext.BaseDirectory, "Files", targetFileName);

            using (FileStream stream = new FileStream(baseFileUrl,
                                              FileMode.Open,
                                              FileAccess.Read))
            {
                FileStream stream1 = new FileStream(targetFileUrl,
                                              FileMode.Create,
                                              FileAccess.Write);
                IWorkbook wb = new XSSFWorkbook(stream);
                ISheet sheet = wb.GetSheet("附件1公议小组预授信评定明细汇总表");

                int rowIndex = 9;

                foreach(var item in appraiseList)
                {
                    var row = sheet.CreateRow(rowIndex);
                    var cell = row.CreateCell(0);
                    // 插入户号
                    cell.SetCellValue(item.HouseHoldId);

                    // 插入无正当职业
                    cell = row.CreateCell(6);
                    cell.SetCellValue(item.Instability ? "是" : "");

                    // 插入欠债不还
                    cell = row.CreateCell(7);
                    cell.SetCellValue(item.Repudiate ? "是" : "");

                    // 插入道德品质较差
                    cell = row.CreateCell(8);
                    cell.SetCellValue(item.Reputation ? "是" : "");

                    // 插入涉嫌赌博行为
                    cell = row.CreateCell(9);
                    cell.SetCellValue(item.Gamble ? "是" : "");

                    // 插入民间借贷
                    cell = row.CreateCell(10);
                    cell.SetCellValue(item.Lending ? "是" : "");

                    // 插入自建平房
                    cell = row.CreateCell(11);
                    if (item.Bungalow)
                    {
                        cell.SetCellValue(item.BungalowCount);
                    }
                    else
                    {
                        cell.SetCellValue(0);
                    }
                    
                    // 插入自建楼房
                    cell = row.CreateCell(12);
                    if (item.Building)
                    {
                        cell.SetCellValue(item.BuildingCount);
                    }
                    else
                    {
                        cell.SetCellValue(0);
                    }
                    
                    // 插入自建别墅
                    cell = row.CreateCell(13);
                    if (item.Cottage)
                    {
                        cell.SetCellValue(item.CottageCount);
                    }
                    else
                    {
                        cell.SetCellValue(0);
                    }

                    // 插入商品房
                    cell = row.CreateCell(14);
                    if(item.GoodsBuilding)
                    {
                        cell.SetCellValue(item.GoodsBuildingCount);
                    }
                    else
                    {
                        cell.SetCellValue(0);
                    }

                    // 插入汽车数量
                    cell = row.CreateCell(15);
                    if (item.CarHold)
                    {
                        cell.SetCellValue(item.CarHoldCount);
                    }
                    else
                    {
                        cell.SetCellValue(0);
                    }

                    // 插入家庭负债
                    cell = row.CreateCell(16);
                    cell.SetCellValue(item.DebtCondition.HasValue ? item.DebtCondition.Value : 0);

                    // 插入家庭年收入
                    cell = row.CreateCell(19);
                    cell.SetCellValue(item.HomeEarning.HasValue ? item.HomeEarning.Value : 0);

                    // 插入家庭年支出
                    cell = row.CreateCell(22);
                    cell.SetCellValue(item.HomePay.HasValue ? item.HomePay.Value : 0);

                    // 插入家庭收入不稳定
                    cell = row.CreateCell(25);
                    cell.SetCellValue(item.Condition1 ? "是" : "");

                    // 插入严重疾病
                    cell = row.CreateCell(26);
                    cell.SetCellValue(item.Condition2 ? "是" : "");

                    // 插入家庭关系不和
                    cell = row.CreateCell(27);
                    cell.SetCellValue(item.Condition3 ? "是" : "");

                    // 插入不遵守村规民约
                    cell = row.CreateCell(28);
                    cell.SetCellValue(item.Condition4 ? "是" : "");

                    // 插入不守信
                    cell = row.CreateCell(29);
                    cell.SetCellValue(item.Condition5 ? "是" : "");

                    // 插入备注
                    cell = row.CreateCell(30);
                    cell.SetCellValue(item.Remarks);


                    // 成员数目大于1时合并单元格发生变化
                    if (item.Members.Count > 1)
                    {
                        // 成员数目大于1时合并户号单元格
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 0, 0));
                        // 合并家庭负债
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 16, 18));
                        // 合并家庭年收入
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 19, 21));
                        // 合并家庭年支出
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 22, 24));

                        // 合并无正当职业
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 6, 6));
                        // 合并欠债不还
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 7, 7));
                        // 合并到的品质较差
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 8, 8));
                        // 合并赌博行为
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 9, 9));
                        // 合并涉嫌高额民间借贷
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 10, 10));
                        // 合并自建平房
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 11, 11));
                        // 合并自建楼房
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 12, 12));
                        // 合并自建别墅
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 13, 13));
                        // 合并商品房
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 14, 14));
                        // 合并汽车
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 15, 15));
                        // 合并家庭收入不稳定
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 25, 25));
                        // 合并存在严重疾病
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 26, 26));
                        // 合并存在家庭关系不和
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 27, 27));
                        // 合并存在不遵守村规民约
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 28, 28));
                        // 合并存在不守信
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 29, 29));
                        // 合并备注
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + item.Members.Count - 1, 30, 30));
                    }
                    else
                    {
                        // 合并家庭负债
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 16, 18));
                        // 合并家庭年收入
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 19, 21));
                        // 合并家庭年支出
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 22, 24));
                    }

                    for(var i = 0; i < item.Members.Count; i++)
                    {
                        if(i != 0)
                        {
                            row = sheet.CreateRow(rowIndex);
                        }

                        // 客户姓名
                        cell = row.CreateCell(1);
                        cell.SetCellValue(item.Members[i].HouseHoldMember.MemberName);

                        // 居住地址
                        cell = row.CreateCell(2);
                        cell.SetCellValue(item.Members[i].HouseHoldMember.Address);

                        // 身份证号
                        cell = row.CreateCell(3);
                        cell.SetCellValue(item.Members[i].HouseHoldMember.IdNumber);

                        // 与户主关系
                        cell = row.CreateCell(4);
                        cell.SetCellValue(item.Members[i].HouseHoldMember.Relationship);

                        // 是否在村内常住
                        cell = row.CreateCell(5);
                        cell.SetCellValue(item.Members[i].Residence);

                        rowIndex += 1;
                    }
                }

                var row1 = sheet.CreateRow(rowIndex);
                var cell1 = row1.CreateCell(1);

                cell1.SetCellValue("备注：1、个人风险情况栏请根据个人评价情况在相应的栏目内打√；2、如客户存在个人风险情况的，在标注√后可不进行其他栏目评价。                              公议小组成员签名：                                           支行三岗签字：");
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 1, 24));

                //var row = sheet.GetRow(9);
                //var cell = row.CreateCell(0);
                //cell.SetCellValue("1");

                //cell = row.GetCell(1);
                //cell.SetCellValue("方勇");

                //cell = row.CreateCell(16);
                //cell.SetCellValue(5);
                //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9, 9, 16, 18));

                //Console.WriteLine(sheet.GetRow(9).GetCell(0).ToString());
                //Console.WriteLine(sheet.GetRow(10).GetCell(0).ToString());
                //Console.WriteLine(sheet.GetRow(11).GetCell(0).ToString());
                //Console.WriteLine(sheet.GetRow(12).GetCell(0).ToString());
                //备注：1、个人风险情况栏请根据个人评价情况在相应的栏目内打√；2、如客户存在个人风险情况的，在标注√后可不进行其他栏目评价。                              公议小组成员签名：                                           支行三岗签字：
                
                wb.Write(stream1);
                wb.Close();
                stream1.Close();
            }

            return ResponseOutput.Ok(targetFileName);
        }

        public async Task<IResponseOutput> ResultCollectAsync(string code)
        {
            var houseList = await _houseHoldRepository.Select
                .Where(i => i.Id.Contains(code))
                .IncludeMany(i => i.Members)
                .Include(i => i.SuggestCreditor)
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

            // 户号
            var col = row.CreateCell(0);
            col.SetCellValue("户号");

            // 成员名称
            col = row.CreateCell(1);
            col.SetCellValue("成员名称");

            // 居住地址
            col = row.CreateCell(2);
            col.SetCellValue("居住地址");

            // 身份证号
            col = row.CreateCell(3);
            col.SetCellValue("身份证号");

            // 与户主关系
            col = row.CreateCell(4);
            col.SetCellValue("与户主关系");

            // 建议授信人姓名
            col = row.CreateCell(5);
            col.SetCellValue("建议授信人姓名");

            // 建议授信人身份证号
            col = row.CreateCell(6);
            col.SetCellValue("建议授信人身份证号");

            // 建议授信额(万元)
            col = row.CreateCell(7);
            col.SetCellValue("建议授信额(万元)");

            // 偏离过大警告
            col = row.CreateCell(8);
            col.SetCellValue("偏离过大警告");

            // 出现一户风险情况认定
            col = row.CreateCell(9);
            col.SetCellValue("出现一户风险情况认定");

            // 拒绝授信表示标识
            col = row.CreateCell(10);
            col.SetCellValue("拒绝授信表示标识");

            // 拒绝授信表示标识
            col = row.CreateCell(11);
            col.SetCellValue("支行确认授信人名字");

            // 拒绝授信表示标识
            col = row.CreateCell(12);
            col.SetCellValue("支行确认授信人身份证号");

            // 拒绝授信表示标识
            col = row.CreateCell(13);
            col.SetCellValue("支行确认授信额度");

            foreach (var item in houseList)
            {
                var memberCount = item.Members.Count;
                row = sheet.CreateRow(rowIndex);

                col = row.CreateCell(0);
                col.SetCellValue(item.Id);

                if(item.SuggestCreditor == null)
                {
                    // 建议授信人姓名
                    col = row.CreateCell(5);
                    col.SetCellValue("");

                    // 建议授信人身份证号
                    col = row.CreateCell(6);
                    col.SetCellValue("");
                }
                else
                {
                    // 建议授信人姓名
                    col = row.CreateCell(5);
                    col.SetCellValue(item.SuggestCreditor.MemberName);

                    // 建议授信人身份证号
                    col = row.CreateCell(6);
                    col.SetCellValue(item.SuggestCreditor.IdNumber);
                }
                

                // 建议授信额(万元)
                col = row.CreateCell(7);
                col.SetCellValue(item.SuggestCreditLimit.HasValue ? item.SuggestCreditLimit.Value : 0);

                // 偏离过大警告
                col = row.CreateCell(8);
                col.SetCellValue(item.DeviationMark);

                // 出现一户风险情况认定
                col = row.CreateCell(9);
                col.SetCellValue(item.DangerUserMark);

                // 拒绝授信表示标识
                col = row.CreateCell(10);
                col.SetCellValue(item.RefuseMark);

                if (memberCount > 1)
                {
                    // 合并户号
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 0, 0));
                    // 合并建议授信人姓名
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 5, 5));
                    // 合并建议授信人身份证号
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 6, 6));
                    // 合并建议授信额
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 7, 7));
                    // 合并偏离过大警告
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 8, 8));
                    // 合并出现一户风险情况认定
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 9, 9));
                    // 合并拒绝授信表示标识
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 10, 10));
                    // 合并支行确认授信人名字
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 11, 11));
                    // 合并支行确认授信人身份证号
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 12, 12));
                    // 合并支行确认授信额度
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex + memberCount - 1, 13, 13));
                }

                for(var i = 0; i < item.Members.Count; i++)
                {
                    if(i != 0)
                    {
                        row = sheet.CreateRow(rowIndex);
                    }

                    // 成员名称
                    col = row.CreateCell(1);
                    col.SetCellValue(item.Members[i].MemberName);

                    // 居住地址
                    col = row.CreateCell(2);
                    col.SetCellValue(item.Members[i].Address);

                    // 身份证号
                    col = row.CreateCell(3);
                    col.SetCellValue(item.Members[i].IdNumber);

                    // 与户主关系
                    col = row.CreateCell(4);
                    col.SetCellValue(item.Members[i].Relationship);

                    rowIndex += 1;
                }
            }
            wb.Write(stream);
            wb.Close();
            stream.Close();

            return ResponseOutput.Ok(fileName);
        }

        public async Task<IResponseOutput> UserPowerSelectAsync(string code)
        {
            var userList = await _userPowerRepository.Select
                .Where(i => i.Power.Contains(code))
                .Include(i => i.User)
                .ToListAsync(i => new { label = i.User.NickName, value = i.UserId });

            return ResponseOutput.Ok(userList);
        }
    }
}
