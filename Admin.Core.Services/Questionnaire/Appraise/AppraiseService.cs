using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Questionnaire;
using Admin.Core.Repository.Questionnaire.Appraise;
using Admin.Core.Repository.Questionnaire.Calculate;
using Admin.Core.Repository.Questionnaire.HouseHold;
using Admin.Core.Repository.Questionnaire.HouseHoldMember;
using Admin.Core.Repository.Questionnaire.MemberResidence;
using Admin.Core.Repository.Questionnaire.UserPower;
using Admin.Core.Service.Questionnaire.Appraise.Input;
using Admin.Core.Service.Questionnaire.Appraise.Output;
using Admin.Core.Service.Questionnaire.HouseHoldMember.Output;
using AutoMapper;

namespace Admin.Core.Service.Questionnaire.Appraise
{
    public class AppraiseService:IAppraiseService
    {
        private readonly IAppraiseRepository _appraiseRepository;
        private readonly IMapper _mapper;
        private readonly IHouseHoldMemberRepository _houseHoldMemberRepository;
        private readonly IMemberResidenceRepository _memberResidenceRepository;
        private readonly IHouseHoldRepository _houseHoldRepository;
        private readonly ICalculateRepository _calculateRepository;
        private readonly IUserPowerRepository _userPowerRepository;
        public AppraiseService(IMapper mapper
            , IAppraiseRepository appraiseRepository
            , IMemberResidenceRepository memberResidenceRepository
            , IHouseHoldMemberRepository houseHoldMemberRepository
            , IHouseHoldRepository houseHoldRepository
            , ICalculateRepository calculateRepository
            , IUserPowerRepository userPowerRepository)
        {
            _mapper = mapper;
            _appraiseRepository = appraiseRepository;
            _houseHoldMemberRepository = houseHoldMemberRepository;
            _memberResidenceRepository = memberResidenceRepository;
            _houseHoldRepository = houseHoldRepository;
            _calculateRepository = calculateRepository;
            _userPowerRepository = userPowerRepository;
        }

        public async Task<IResponseOutput> AddInfoReturnAsync(string id)
        {
            var members = await _houseHoldMemberRepository.Select
                .Where(i => i.HouseHoldId == id)
                .ToListAsync();

            var appraise = new AppraiseAddOutput()
            {
                HouseHoldId = id,
                Members = _mapper.Map<List<HouseHoldMemberAddOutput>>(members)
            };

            return ResponseOutput.Ok(appraise);
        }

        public async Task<IResponseOutput> AddAsync(AppraiseAddInput input, long userId)
        {
            var entity = _mapper.Map<AppraiseEntity>(input);
            entity.AppraiserId = userId;

            var obj = await _appraiseRepository.InsertAsync(entity);

            var memberResidenceList = _mapper.Map<List<MemberResidenceEntity>>(input.Members);

            foreach(var item in memberResidenceList)
            {
                item.AppraiseId = obj.Id;
                await _memberResidenceRepository.InsertAsync(item);
            }

            var appraiseCount = await _appraiseRepository.Select
                .Where(i => i.HouseHoldId == input.HouseHoldId)
                .CountAsync();
            if(appraiseCount == 5)
            {
                // 建议授信人计算
                long user = 0;
                var grade = 0;
                var userGrade = 0;

                var memberList = await _houseHoldMemberRepository.Select
                    .Where(i => i.HouseHoldId == input.HouseHoldId)
                    .IncludeMany(i => i.MemberResidences)
                    .ToListAsync();

                foreach (var item in memberList)
                {
                    var residenceList = item.MemberResidences.Select(i => i.Residence);
                    grade = 0;
                    // 是否常住 超过三人判定在村里即认为常住村里
                    if(residenceList.Where(i => i == true).Count() >= 3)
                    {
                        grade += 30;
                    }
                    if (item.BirthDate.HasValue)
                    {
                        // 计算年龄
                        int age = DateTime.Now.Year - item.BirthDate.Value.Year;
                        if (DateTime.Now.Month < item.BirthDate.Value.Month || (DateTime.Now.Month == item.BirthDate.Value.Month && DateTime.Now.Day < item.BirthDate.Value.Day))
                        {
                            age--;
                        }
                        if (18 <= age && age <= 25)
                        {
                            grade += 10;
                        }
                        else if (26 <= age && age <= 35)
                        {
                            grade += 20;
                        }
                        else if (36 <= age && age <= 45)
                        {
                            grade += 30;
                        }
                        else if (46 <= age && age <= 55)
                        {
                            grade += 20;
                        }
                        else if (56 <= age && age <= 65)
                        {
                            grade += 10;
                        }
                    }

                    // 有丰收互联
                    if (item.OwnedHarvestInternet)
                    {
                        grade += 10;
                    }
                    // 有借记卡
                    if (item.OwnedDebitCard)
                    {
                        grade += 20;
                    }

                    if(userGrade < grade)
                    {
                        user = item.Id;
                        userGrade = grade;
                    }
                }
                await _houseHoldRepository.UpdateDiy.Set(i => i.SuggestCreditorId, user).Where(i => i.Id == input.HouseHoldId).ExecuteAffrowsAsync();

                // 建议授信额度测算
                float basicQuota = 0.00F;
                float familyNetIncome = 0.00F;
                float familyNetWorthy = 0.00F;
                var Coefficient = 70;
                var dangerCount = 0;
                var quotaList = new List<float>();
                // 承包亩数
                var houseHoldEntity = await _houseHoldRepository.Select
                    .Where(i => i.Id == input.HouseHoldId)
                    .ToOneAsync();
                var acres = houseHoldEntity.Acres.HasValue ? houseHoldEntity.Acres.Value : 0;  

                var appraiseList = await _appraiseRepository.Select
                    .Where(i => i.HouseHoldId == input.HouseHoldId)
                    .ToListAsync();

                foreach(var item in appraiseList)
                {
                    basicQuota = 0.00F;
                    Coefficient = 70;
                    familyNetWorthy = 0.00F;

                    if(item.Instability || item.Repudiate || item.Reputation || item.Lending || item.Gamble)
                    {
                        dangerCount += 1;
                        continue;
                    }

                    if(dangerCount >= 2)
                    {
                        break;
                    }

                    // 承包土地价值计算
                    familyNetWorthy += (float)(acres * 0.72);

                    // 自有房产价值计算
                    if (item.SelfBuilding)
                    {
                        if (item.Bungalow)
                        {
                            familyNetWorthy += item.BungalowCount * 20;
                        }
                        if (item.Building)
                        {
                            familyNetWorthy += item.BuildingCount * 40;
                        }
                        if (item.Cottage)
                        {
                            familyNetWorthy += item.CottageCount * 60;
                        }
                    }
                    // 商品房价值计算
                    if (item.GoodsBuilding)
                    {
                        familyNetWorthy += item.GoodsBuildingCount * 60;
                    }
                    // 车辆价值计算
                    if (item.CarHold)
                    {
                        familyNetWorthy += item.CarHoldCount * 5;
                    }

                    // 家庭净资产计算
                    if (item.DebtCondition.HasValue)
                    {
                        familyNetWorthy -= item.DebtCondition.Value;
                    }

                    // 家庭年净收入
                    if (item.HomeEarning.HasValue && item.HomePay.HasValue)
                    {
                        familyNetIncome = item.HomeEarning.Value - item.HomePay.Value;
                    }

                    // 取高值
                    basicQuota = (float)(familyNetIncome * 1.5 < familyNetWorthy * 0.3 ? familyNetWorthy * 0.3 : familyNetIncome * 1.5);

                    // 负值判断
                    if (basicQuota < 0)
                    {
                        basicQuota = 0;
                    }

                    // 计算评价系数
                    if (item.Condition1)
                    {
                        Coefficient -= 20;
                    }
                    if (item.Condition2)
                    {
                        Coefficient -= 20;
                    }
                    if (item.Condition3)
                    {
                        Coefficient -= 10;
                    }
                    if (item.Condition4)
                    {
                        Coefficient -= 10;
                    }
                    if (item.Condition5)
                    {
                        Coefficient -= 10;
                    }

                    // 添加到列表中
                    quotaList.Add(basicQuota * Coefficient / 70);
                }

                if(dangerCount >= 2)
                {
                    await _houseHoldRepository.UpdateDiy.Set(i => i.RefuseMark, true).Where(i => i.Id == input.HouseHoldId).ExecuteAffrowsAsync();
                    var calEntity = new CalculateEntity()
                    {
                        HouseHoldId = input.HouseHoldId,
                        RefuseMark = true
                    };
                    await _calculateRepository.InsertAsync(calEntity);
                }
                else
                {
                    // 偏离度计算
                    //var maxValue = quotaList.Max();
                    //var minValue = quotaList.Min();
                    var sign = false;
                    // 风险情况判定
                    var riskSign = false;

                    //quotaList.Remove(maxValue);
                    //quotaList.Remove(minValue);

                    // 五个值求取平均值,求取标准差
                    //float total = 0.00F;
                    double avg = quotaList.Average();
                    double sum = quotaList.Sum(d => Math.Pow(d - avg, 2));
                    double ret = Math.Sqrt(sum / quotaList.Count());

                    // 计算变异系数
                    if (ret / avg > 0.5)
                    //if (maxValue - minValue > average * 0.5)
                    {
                        sign = true;
                    }
                    if (dangerCount == 1)
                    {
                        riskSign = true;
                    }

                    var calEntity = new CalculateEntity()
                    {
                        HouseHoldId = input.HouseHoldId,
                        Average = avg,
                        StandardDeviation = ret,
                        Deviation = ret / avg,
                        VarianceSum = sum,
                        DangerUserMark = riskSign,
                        DeviationMark = sign
                    };

                    // 授信额度大于30万设置30万封顶
                    if (avg > 30)
                    {
                        avg = 30;
                    }
                    else
                    {
                        // 取整,四舍六入
                        avg = Math.Floor(avg);
                    }

                    //float total = 0.00F;
                    //quotaList.ForEach(i => total += i);
                    //float average = total / quotaList.Count();

                    await _houseHoldRepository.UpdateDiy
                        .Set(i => i.SuggestCreditLimit, (float)avg)
                        .Set(i => i.DeviationMark, sign)
                        .Set(i => i.DangerUserMark, riskSign)
                        .Where(i => i.Id == input.HouseHoldId).ExecuteAffrowsAsync();

                    await _calculateRepository.InsertAsync(calEntity);
                }
            }

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> PageAsync(PageInput<AppraiseEntity> input, long userId)
        {
            var list = await _appraiseRepository.Select
                .Where(i => i.AppraiserId == userId)
                .Include(i => i.HouseHold)
                .OrderBy(i => i.HouseHold.BelongedStreet)
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var data = new PageOutput<AppraisePageOutput>()
            {
                Total = total,
                List = _mapper.Map<List<AppraisePageOutput>>(list)
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> DetailAsync(long id)
        {
            var entity = await _appraiseRepository.Select
                .WhereDynamic(id)
                .IncludeMany(i => i.Members, then => then.Include(n => n.HouseHoldMember))
                .ToOneAsync();

            var data = _mapper.Map<AppraiseDetailOutput>(entity);

            return ResponseOutput.Ok(data);
        }
    }
}
