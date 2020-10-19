using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Model.Record;
using Admin.Core.Repository.Record.CheckedRecordFile;
using Admin.Core.Repository.Record.Record;
using Admin.Core.Repository.Record.RecordBorroItem;
using Admin.Core.Repository.Record.RecordBorrow;
using Admin.Core.Repository.Record.RecordHistory;
using Admin.Core.Service.Record.RecordBorrow.Input;
using Admin.Core.Service.Record.RecordBorrow.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordBorrow
{
    public class RecordBorrowService:IRecordBorrowService
    {
        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly IRecordRepository _recordRepository;
        private readonly IRecordBorrowRepository _recordBorrowRepository;
        private readonly IRecordHistoryRepository _recordHistoryRepository;
        private readonly IRecordBorrowItemRepository _recordBorrowItemRepository;
        private readonly ICheckedRecordFileRepository _checkedRecordFileRepostiory;
        public RecordBorrowService(IMapper mapper
            , IUser user
            , IRecordBorrowRepository recordBorrowRepository
            , IRecordHistoryRepository recordHistoryRepository
            , IRecordRepository recordRepository
            , IRecordBorrowItemRepository recordBorrowItemRepository
            , ICheckedRecordFileRepository checkedRecordFileRepository)
        {
            _mapper = mapper;
            _user = user;
            _recordBorrowRepository = recordBorrowRepository;
            _recordHistoryRepository = recordHistoryRepository;
            _recordRepository = recordRepository;
            _recordBorrowItemRepository = recordBorrowItemRepository;
            _checkedRecordFileRepostiory = checkedRecordFileRepository;
        }

        [Transaction]
        public async Task<IResponseOutput> BorrowOrReadAsync(RecordBorrowAddInput input)
        {
            var recordBorrowEntity = new RecordBorrowEntity()
            {
                UserId = _user.Id,
                BorrowType = input.BorrowType,
                CreatedTime = DateTime.Now
            };
            var statuList = await _recordRepository.Select.Where(i => input.RecordBorrowItemIdList.Contains(i.Id)).ToListAsync(i => i.Status);
            if (statuList.Contains(0) || statuList.Contains(2))
            {
                return ResponseOutput.NotOk("其中有不是在库状态的档案，请刷新后重试!");
            }
            var added = await _recordBorrowRepository.InsertAsync(recordBorrowEntity);
            foreach (var recordId in input.RecordBorrowItemIdList)
            {
                var record = await _recordRepository.Select.WhereDynamic(recordId).ToOneAsync();
                var recordHistory = new RecordHistoryEntity()
                {
                    RecordId = recordId,
                    OperateType = input.BorrowType == 0 ? "申请借阅" : "申请调阅",
                    OperateInfo = input.BorrowType == 0 ? "申请借阅" : "申请调阅" + $"{record.RecordUserName}-{record.RecordUserInCode}档案"
                };
                
                var recordBorrowItem = new RecordBorrowItemEntity()
                {
                    RecordId = recordId,
                    RecordBorrowId = added.Id
                };
                await _recordBorrowItemRepository.InsertAsync(recordBorrowItem);
                await _recordHistoryRepository.InsertAsync(recordHistory);
            }
            
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> CancelBorrowOrReadAsync(long id)
        {
            var recordBorrow = await _recordBorrowRepository.Select.WhereDynamic(id).IncludeMany(i => i.RecordBorrowItemList, then => then.Include(a => a.Record)).ToOneAsync();

            if(recordBorrow.ReturnSign == 0)
            {
                foreach (var recordBorrowItem in recordBorrow.RecordBorrowItemList)
                {
                    var recordHistory = new RecordHistoryEntity()
                    {
                        RecordId = recordBorrowItem.Record.Id,
                        OperateType = recordBorrow.BorrowType == 0 ? "取消申请借阅" : "取消申请调阅",
                        OperateInfo = recordBorrow.BorrowType == 0 ? "取消申请借阅" : "取消申请调阅" + $"{recordBorrowItem.Record.RecordUserName}-{recordBorrowItem.Record.RecordUserInCode}档案"
                    };

                    
                    await _recordHistoryRepository.InsertAsync(recordHistory);
                }
                var item = await _recordBorrowRepository.UpdateDiy.Set(i => i.ReturnSign, 3).Where(i => i.Id == recordBorrow.Id).ExecuteAffrowsAsync();

                return ResponseOutput.Ok();
            }
            else
            {
                return ResponseOutput.NotOk();
            }
            
        }

        public async Task<IResponseOutput> GetBorrowListAsync()
        {
            var borrowList = await _recordBorrowRepository.Select
                .Where(i => i.UserId == _user.Id)
                .IncludeMany(i => i.RecordBorrowItemList, then => then.Include(a => a.Record))
                .OrderByDescending(i => i.CreatedTime)
                .ToListAsync();

            return ResponseOutput.Ok(borrowList);
        }

        public async Task<IResponseOutput> GetVerifyListAsync(PageInput<RecordBorrowEntity> input)
        {
            var entityList = await _recordBorrowRepository.Select
                .Where(i => i.ReturnSign == 0)
                .IncludeMany(i => i.RecordBorrowItemList, then => then.Include(a => a.Record))
                .Include(i => i.User)
                .IncludeMany(i => i.User.Departments)
                .Count(out var total)
                .OrderByDescending(i => i.CreatedTime)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var data = new PageOutput<RecordBorrowOutput>()
            {
                List = _mapper.Map<List<RecordBorrowOutput>>(entityList),
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> GetBorrowDetailAsync(long id)
        {
            var entityList = await _recordBorrowRepository.Select
                .Where(i => i.ReturnSign == 0 && i.Id == id)
                .IncludeMany(i => i.RecordBorrowItemList, then => then.Include(a => a.Record))
                .Include(i => i.User)
                .IncludeMany(i => i.User.Departments)
                .ToOneAsync();

            var output = _mapper.Map<RecordBorrowDetailOutput>(entityList);

            return ResponseOutput.Ok(output);
        }

        [Transaction]
        public async Task<IResponseOutput> VerifyBorrowAsync(RecordBorrowVerifyInput input)
        {
            var recordBorrow = await _recordBorrowRepository.Select
                .WhereDynamic(input.Id)
                .IncludeMany(i => i.RecordBorrowItemList, then => then.Include(a => a.Record))
                .ToOneAsync();

            if(recordBorrow.ReturnSign != 0)
            {
                return ResponseOutput.NotOk("用户已取消申请，审核失败，请刷新后重试!");
            }
            else
            {
                //审核拒绝
                if(input.VerifyType == 0)
                {
                    recordBorrow.ReturnSign = 2;
                    foreach(var item in recordBorrow.RecordBorrowItemList)
                    {
                        var recordHistory = new RecordHistoryEntity()
                        {
                            RecordId = item.Record.Id,
                            OperateType = "审核拒绝",
                            OperateInfo = $"档案 {item.Record.RecordUserName}-{item.Record.RecordUserInCode}审核被拒 <br> 拒绝原因:{input.RefuseReason}"
                        };

                        await _recordHistoryRepository.InsertAsync(recordHistory);
                    }

                    if (!string.IsNullOrEmpty(input.RefuseReason))
                    {
                        recordBorrow.RefuseReason = input.RefuseReason;
                    }

                    await _recordBorrowRepository.UpdateAsync(recordBorrow);

                    return ResponseOutput.Ok();
                }
                //审核同意
                else if(input.VerifyType == 1)
                {
                    recordBorrow.ReturnSign = 1;
                    var sign = false;
                    foreach (var item in recordBorrow.RecordBorrowItemList)
                    {
                        //确定借调阅档案是在库状态
                        if(item.Record.Status != 1 && item.Record.Status != 3)
                        {
                            sign = true;
                            break;
                        }
                    }
                    //档案全部在库
                    if(!sign)
                    {
                        foreach (var item in recordBorrow.RecordBorrowItemList)
                        {
                            var recordHistory = new RecordHistoryEntity()
                            {
                                RecordId = item.Record.Id,
                                OperateType = "审核同意",
                                OperateInfo = $"档案 {item.Record.RecordUserName}-{item.Record.RecordUserInCode}审核通过"
                            };

                            await _recordHistoryRepository.InsertAsync(recordHistory);
                            if(item.Record.Status != 3)
                            {
                                item.Record.Status = 2;
                                await _recordRepository.UpdateAsync(item.Record);
                            }

                            // 更改选中文件的状态为借阅状态
                            if (recordBorrow.BorrowType == 0)
                            {
                                await _checkedRecordFileRepostiory.UpdateDiy.Set(i => i.HandOverSign, 3).Where(i => i.RecordId == item.RecordId && i.HandOverSign == 1).ExecuteAffrowsAsync();
                            }
                        }

                        await _recordBorrowRepository.UpdateAsync(recordBorrow);

                        return ResponseOutput.Ok();
                    }
                    //档案有不在库的情况
                    else
                    {
                        recordBorrow.ReturnSign = 2;
                        recordBorrow.RefuseReason = "当前审核档案中有不在库的档案，审核失败";
                        await _recordBorrowRepository.UpdateAsync(recordBorrow);
                        return ResponseOutput.NotOk("当前审核档案中有不在库的档案，审核失败");
                    }

                }
                //未知状态字
                else
                {
                    return ResponseOutput.NotOk("输入审核状态字出错，请刷新后重试!");
                }

            }
        }

        public async Task<IResponseOutput> GetReturnPageAsync(PageInput<RecordBorrowEntity> input)
        {
            var entityList = await _recordBorrowRepository.Select
                .Where(i => i.ReturnSign == 1)
                .IncludeMany(i => i.RecordBorrowItemList, then => then.Include(a => a.Record))
                .Include(i => i.User)
                .IncludeMany(i => i.User.Departments)
                .Count(out var total)
                .OrderByDescending(i => i.CreatedTime)
                .ToListAsync();

            var output = new PageOutput<ReturnPageOutput>()
            {
                Total = total,
                List = _mapper.Map<List<ReturnPageOutput>>(entityList)
            };

            return ResponseOutput.Ok(output);
        }

        [Transaction]
        public async Task<IResponseOutput> ReturnRecordAsync(long id)
        {
            var recordBorrowEntity = await _recordBorrowRepository.Select
                .WhereDynamic(id)
                .IncludeMany(i => i.RecordBorrowItemList, then => then.Include(a => a.Record))
                .ToOneAsync();

            foreach(var item in recordBorrowEntity.RecordBorrowItemList)
            {
                var recordHistory = new RecordHistoryEntity()
                {
                    RecordId = item.Record.Id,
                    OperateType = "借阅归还",
                    OperateInfo = $" 档案{item.Record.RecordUserName}-{item.Record.RecordUserInCode}归还接收"
                };
                var count = await _checkedRecordFileRepostiory.Select
                    .Where(i => i.RecordId == item.Record.Id && i.HandOverSign == 2)
                    .CountAsync();
                if(count > 0)
                {
                    item.Record.Status = 3;
                }
                else
                {
                    item.Record.Status = 1;
                }
                
                await _recordRepository.UpdateAsync(item.Record);
                await _recordHistoryRepository.InsertAsync(recordHistory);
                // 更改选中档案文件状态从借阅中为已移交状态
                await _checkedRecordFileRepostiory.UpdateDiy.Set(i => i.HandOverSign, 1).Where(i => i.RecordId == item.RecordId && i.HandOverSign == 3).ExecuteAffrowsAsync();
            }

            recordBorrowEntity.ReturnSign = 4;
            await _recordBorrowRepository.UpdateAsync(recordBorrowEntity);

            return ResponseOutput.Ok();
        }

        public async Task<UserEntity> GetUserByBorrowId(long id)
        {
            var entity = await _recordBorrowRepository.Select
                .WhereDynamic(id)
                .Include(i => i.User)
                .ToOneAsync(i => i.User);

            return entity;
        }
    }
}
