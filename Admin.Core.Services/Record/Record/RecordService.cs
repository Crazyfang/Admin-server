using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Enums;
using Admin.Core.Model.Admin;
using Admin.Core.Model.Record;
using Admin.Core.Repository.Admin;
using Admin.Core.Repository.Admin.Department;
using Admin.Core.Repository.Admin.RecordHistory;
using Admin.Core.Repository.Record.CheckedRecordFile;
using Admin.Core.Repository.Record.CheckedRecordFileType;
using Admin.Core.Repository.Record.InitiativeUpdate;
using Admin.Core.Repository.Record.initiativeUpdateItem;
using Admin.Core.Repository.Record.Notify;
using Admin.Core.Repository.Record.Record;
using Admin.Core.Repository.Record.RecordFile;
using Admin.Core.Repository.Record.RecordFileType;
using Admin.Core.Repository.Record.RecordId;
using Admin.Core.Service.Record.CheckedRecordFile.Input;
using Admin.Core.Service.Record.InitiativeUpdateItem.Output;
using Admin.Core.Service.Record.Record.InitiativeUpdate.Output;
using Admin.Core.Service.Record.Record.Input;
using Admin.Core.Service.Record.Record.Output;
using Admin.Core.Service.Record.RecordFile.Output;
using Admin.Core.Service.Record.RecordFileType.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.Record
{
    public class RecordService : IRecordService
    {
        private readonly IMapper _mapper;
        private readonly IRecordRepository _recordRepository;
        private readonly ICheckedRecordFileRepository _checkedRecordFileRepository;
        private readonly IRecordFileTypeRepository _recordFileTypeRepository;
        private readonly IRecordHistoryRepository _recordHistoryRepository;
        private readonly IRecordFileRepository _recordFileRepository;
        private readonly ICheckedRecordFileTypeRepository _checkedRecordFileTypeRepository;
        private readonly IRecordIdRepository _recordIdRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInitiativeUpdateRepository _initiativeUpdateRepository;
        private readonly IInitiativeUpdateItemRepository _initiativeUpdateItemRepository;
        private readonly IFreeSql _freeSql;
        private readonly RedisDataHelper _redisDataHelper;
        private readonly INotifyRepository _notifyRepository;

        public RecordService(IMapper mapper
            , IRecordRepository recordRepository
            , ICheckedRecordFileRepository checkedRecordFileRepository
            , IRecordFileTypeRepository recordFileTypeRepository
            , IRecordHistoryRepository recordHistoryRepository
            , IRecordFileRepository recordFileRepository
            , ICheckedRecordFileTypeRepository checkedRecordFileTypeRepository
            , IRecordIdRepository recordIdRepository
            , IDepartmentRepository departmentRepository
            , IUserRepository userRepository
            , IInitiativeUpdateRepository initiativeUpdateRepository
            , IInitiativeUpdateItemRepository initiativeUpdateItemRepository
            , IFreeSql freeSql
            , RedisDataHelper redisDataHelper
            , INotifyRepository notifyRepository)
        {
            _mapper = mapper;
            _recordRepository = recordRepository;
            _checkedRecordFileRepository = checkedRecordFileRepository;
            _recordFileTypeRepository = recordFileTypeRepository;
            _recordHistoryRepository = recordHistoryRepository;
            _recordFileRepository = recordFileRepository;
            _checkedRecordFileTypeRepository = checkedRecordFileTypeRepository;
            _recordIdRepository = recordIdRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _initiativeUpdateRepository = initiativeUpdateRepository;
            _initiativeUpdateItemRepository = initiativeUpdateItemRepository;
            _freeSql = freeSql;
            _redisDataHelper = redisDataHelper;
            _notifyRepository = notifyRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var entity = await _recordRepository.Select
                .WhereDynamic(id)
                .Include(a => a.ManagerDepartment)
                .Include(a => a.ManagerUser)
                .ToOneAsync();

            var entityDto = _mapper.Map<RecordGetOutput>(entity);

            return ResponseOutput.Ok(entityDto, "编辑界面");
        }

        public async Task<RecordEntity> GetRecordAsync(long id)
        {
            var entity = await _recordRepository.Select
                .WhereDynamic(id)
                .Include(a => a.ManagerDepartment)
                .Include(a => a.ManagerUser)
                .ToOneAsync();

            return entity;
        }

        public async Task<long> GetRecordTypeAsync(long id)
        {
            var record = await _recordRepository.GetAsync(id);
            if (!(record?.Id > 0))
            {
                return 0;
            }
            else
            {
                return record.RecordType;
            }
        }

        public async Task<IResponseOutput> GetRecordInfoAsync(long id)
        {
            var output = new RecordBasicInfoOutput();
            var entity = await _recordRepository.Select
                .WhereDynamic(id)
                .Include(a => a.ManagerDepartment)
                .Include(a => a.ManagerUser)
                .ToOneAsync();

            output.Record = _mapper.Map<RecordGetOutput>(entity);

            var entityList = await _checkedRecordFileTypeRepository.Select
                .Where(i => i.RecordId == id)
                .Include(i => i.RecordFileType)
                .ToListAsync(i => new RecordFileTypeOutput() { RecordFileTypeId = i.RecordFileTypeId, Name = i.RecordFileType.FileTypeName, Remarks = i.Remarks, CheckedRecordFileTypeId = i.Id });

            //var entityList = await _recordFileTypeRepository.Select.From<CheckedRecordFileTypeEntity>((s, b) => s.LeftJoin(a => a.Id == b.RecordFileTypeId))
            //    .Where((s, b) => s.RecordTypeId == entity.RecordType)
            //    .ToListAsync((s, b) => new RecordFileTypeOutput() { RecordFileTypeId = s.Id, Name = s.FileTypeName, Remarks = b.Remarks, CheckedRecordFileTypeId = b.Id });

            output.RecordFileTypeList = entityList;

            foreach (var item in output.RecordFileTypeList)
            {
                var checkedRecordFileList = await _checkedRecordFileRepository.Select
                    .Where(i => i.RecordId == entity.Id && i.CheckedRecordFileTypeId == item.CheckedRecordFileTypeId)
                    .ToListAsync();
                //var checkedRecordFileType = await _checkedRecordFileTypeRepository.Select
                //    .Where(i => i.RecordId == entity.Id && i.RecordFileTypeId == item.RecordFileTypeId && i.Id == item.CheckedRecordFileTypeId)
                //    .IncludeMany(i => i.CheckedRecordFileList, then => then.Include(a => a.RecordFile))
                //    .ToOneAsync();

                //if (checkedRecordFileType != null)
                //{
                //    item.Children = _mapper.Map<List<CheckedRecordFileInput>>(checkedRecordFileType.CheckedRecordFileList);
                //}
                item.Children = _mapper.Map<List<CheckedRecordFileInput>>(checkedRecordFileList);
            }

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> GetRecordAdditionalInfoAsync(long id)
        {
            var output = new RecordAdditionalInfoOutput();
            var entity = await _recordRepository.Select.WhereDynamic(id).Include(i => i.ManagerDepartment).Include(i => i.ManagerUser).ToOneAsync();
            output.Record = _mapper.Map<RecordGetOutput>(entity);

            var entityList = await _recordFileTypeRepository.Select
                .Where(a => a.RecordTypeId == entity.RecordType)
                .ToListAsync(a => new RecordFileTypeAdditionalOutput() { RecordFileTypeId = a.Id, Name = a.FileTypeName, HasContractNo = a.HasContractNo });

            var checkedRecordFileTypeList = await _checkedRecordFileTypeRepository.Select
                .Where(a => a.RecordId == entity.Id)
                .Include(i => i.RecordFileType)
                .ToListAsync();

            for (var i = entityList.Count - 1; i >= 0; i--)
            {
                foreach (var item in checkedRecordFileTypeList)
                {
                    if (entityList[i].RecordFileTypeId == item.RecordFileTypeId)
                    {
                        if (entityList[i].CheckedRecordFileTypeId == 0)
                        {
                            entityList[i].CheckedRecordFileTypeId = item.Id;
                            entityList[i].Remarks = item.Remarks;
                        }
                        else
                        {
                            var obj = new RecordFileTypeAdditionalOutput()
                            {
                                RecordFileTypeId = entityList[i].RecordFileTypeId,
                                Name = entityList[i].Name,
                                CheckedRecordFileTypeId = item.Id,
                                Remarks = item.Remarks,
                                HasContractNo = item.RecordFileType.HasContractNo
                            };
                            entityList.Insert(i + 1, obj);
                        }
                    }
                }
            }
            foreach(var item in entityList)
            {
                var recordFileList = await _recordFileRepository.Select.Where(a => a.RecordFileTypeId == item.RecordFileTypeId).ToListAsync();
                var checkedRecordFileList = await _checkedRecordFileRepository.Select
                    .Where(i => i.HandOverSign == 0 || i.HandOverSign == 2)
                    .Where(i => i.RecordId == id)
                    .Where(i => i.CheckedRecordFileTypeId == item.CheckedRecordFileTypeId)
                    .ToListAsync();
                var additionalRecordFileList = _mapper.Map<List<RecordFileAdditionalOuput>>(recordFileList);

                foreach(var file in additionalRecordFileList)
                {
                    foreach(var checkedFile in checkedRecordFileList)
                    {
                        if(file.RecordFileId == checkedFile.RecordFileId)
                        {
                            file.Checked = true;
                            file.Num = checkedFile.Num;
                            file.CreditDueDate = checkedFile.CreditDueDate;
                            file.Id = checkedFile.Id;
                        }
                    }
                }

                foreach(var otherFile in checkedRecordFileList.Where(i => i.OtherSign == 1))
                {
                    var otherEntity = new RecordFileAdditionalOuput()
                    {
                        Id = otherFile.Id,
                        Num = otherFile.Num,
                        Name = otherFile.Name,
                        CreditDueDate = otherFile.CreditDueDate,
                        Checked = true,
                        OtherSign = otherFile.OtherSign,
                        CheckedRecordFileId = otherFile.Id,
                        HandOverSign = otherFile.HandOverSign
                    };
                    additionalRecordFileList.Add(otherEntity);
                }

                item.Children = additionalRecordFileList;
            }

            output.RecordFileTypeList = entityList;

            return ResponseOutput.Ok(output, "补充提交界面");
        }

        public async Task<IResponseOutput> PageAsync(PageInput<RecordEntity> input)
        {
            var list = await _recordRepository.Select
            .WhereIf(input.Filter.ManagerUserId.HasValue, a => a.ManagerUserId == input.Filter.ManagerUserId)
            .WhereIf(input.Filter.RecordId.NotNull(), a => a.RecordId.Contains(input.Filter.RecordId))
            .WhereIf(input.Filter.RecordUserName.NotNull(), a => a.RecordUserName.Contains(input.Filter.RecordUserName))
            .WhereIf(input.Filter.RecordUserInCode.NotNull(), a => a.RecordUserInCode.Contains(input.Filter.RecordUserInCode))
            .WhereIf(input.Filter.RecordUserCode.NotNull(), a => a.RecordUserCode.Contains(input.Filter.RecordUserCode))
            .WhereIf(input.Filter.ManagerDepartmentId.HasValue, a => a.ManagerDepartmentId == input.Filter.ManagerDepartmentId)
            .Count(out var total)
            .OrderByDescending(true, a => a.Id)
            .Include(a => a.ManagerDepartment)
            .Include(a => a.ManagerUser)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<RecordListOutput>()
            {
                List = _mapper.Map<List<RecordListOutput>>(list),
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        [Transaction]
        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var record = await _recordRepository.Select.WhereDynamic(id).ToOneAsync();
            if (record.Status == 0)
            {
                var recordHistory = new RecordHistoryEntity()
                {
                    RecordId = id,
                    OperateType = "删除",
                    OperateInfo = $"删除{record.RecordUserName}档案"
                };
                //await _recordRepository.DeleteAsync(id);
                //var checkedFileIdList = await _checkedRecordFileRepository.Select
                //    .Where(i => i.RecordId == id)
                //    .ToListAsync(i => i.Id);
                //checkedFileIdList = checkedFileIdList.Count > 0 ? checkedFileIdList : new List<long>() { 0 };

                await _recordRepository.DeleteAsync(i => i.Id == id);
                await _checkedRecordFileRepository.DeleteAsync(i => i.RecordId == id);
                await _checkedRecordFileTypeRepository.DeleteAsync(i => i.RecordId == id);
                await _recordHistoryRepository.InsertAsync(recordHistory);

                return ResponseOutput.Ok();
            }
            else
            {
                return ResponseOutput.NotOk("无法删除待移交状态的档案！");
            }
        }

        [Transaction]
        public async Task<IResponseOutput> AddAsync(RecordAddInput input, List<RecordFileTypeOutput> fileInput)
        {
            var entity = _mapper.Map<RecordEntity>(input);

            var existEntity = await _recordRepository.Select
                .Where(i => i.RecordUserCode == entity.RecordUserCode || i.RecordUserInCode == entity.RecordUserInCode)
                .Where(i => i.ManagerDepartmentId == entity.ManagerDepartmentId)
                .Count(out var count)
                .ToOneAsync();

            if(count != 0)
            {
                return ResponseOutput.NotOk("同个支行下存在相同客户内码或者客户号的客户，不允许重复添加!");
            }

            var ins = new RecordIdEntity()
            {
                msg = "获取id"
            };

            var departmentEntity = await _departmentRepository.Select.WhereDynamic(input.ManagerDepartmentId).ToOneAsync();
            var redisKey = Enum.GetName(typeof(DepartmentCode), departmentEntity.DepartmentCode);
            var seedValue = _redisDataHelper.IncrIndex(redisKey);
            var recordId = $"AAAA{departmentEntity.DepartmentCode}{seedValue.ToString().PadLeft(5, '0')}";

            //var recordIdEntity = await _recordIdRepository.InsertAsync(ins);
            //var department = await _departmentRepository.Select.WhereDynamic(entity.ManagerDepartmentId).ToOneAsync();
            //var recordId = $"AAAA{department.DepartmentCode}{recordIdEntity.Id.ToString().PadLeft(5, '0')}";
            entity.RecordId = recordId;
            var record = await _recordRepository.InsertAsync(entity);

            if (!(record?.Id > 0))
            {
                return ResponseOutput.NotOk("发生错误，请刷新后重试");
            }
            else
            {
                var recordHistory = new RecordHistoryEntity()
                {
                    RecordId = record.Id,
                    OperateType = "新增",
                    OperateInfo = $"创建了{record.RecordUserName}档案"
                };
                foreach (var recordFileTypeInput in fileInput)
                {
                    var recordFileList = recordFileTypeInput.Children.Where(i => i.Checked == true).ToList();
                    if (recordFileList.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        var checkedRecordFileTypeEntity = _mapper.Map<CheckedRecordFileTypeEntity>(recordFileTypeInput);
                        checkedRecordFileTypeEntity.RecordId = record.Id;
                        var checkedRecordFileType = await _checkedRecordFileTypeRepository.InsertAsync(checkedRecordFileTypeEntity);

                        if (!(checkedRecordFileType.Id > 0))
                        {
                            return ResponseOutput.NotOk();
                        }
                        // 档案文件处理
                        foreach (var checkedRecordFile in recordFileTypeInput.Children.Where(i => i.Checked == true).ToList())
                        {
                            var checkedRecordFileEntity = _mapper.Map<CheckedRecordFileEntity>(checkedRecordFile);
                            checkedRecordFileEntity.RecordId = record.Id;
                            checkedRecordFileEntity.CheckedRecordFileTypeId = checkedRecordFileType.Id;
                            var returnModel = await _checkedRecordFileRepository.InsertAsync(checkedRecordFileEntity);
                            var recordFile = await _recordFileRepository.GetAsync(returnModel.RecordFileId);
                            if (checkedRecordFileEntity.OtherSign == 0)
                            {
                                recordHistory.OperateInfo += $"<br> 选中档案文件:{recordFileTypeInput.Name}-{recordFile.RecordFileName} 份数:{returnModel.Num}";
                            }
                            else if (checkedRecordFileEntity.OtherSign == 1)
                            {
                                recordHistory.OperateInfo += $"<br> 用户自定义文件:{checkedRecordFileEntity.Name} 份数:{returnModel.Num}";
                            }
                        }
                    }
                }
                await _recordHistoryRepository.InsertAsync(recordHistory);
            }

            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> UpdateAsync(RecordUpdateInput input, List<RecordFileTypeUpdateOutput> fileInput)
        {
            var recordEntity = await _recordRepository.GetAsync(input.Id);
            if (!(recordEntity?.Id > 0))
            {
                return ResponseOutput.NotOk("当前编辑档案主键不存在，请刷新后重试!");
            }
            else
            {
                var typeId = recordEntity.RecordType;
                _mapper.Map(input, recordEntity);
                await _recordRepository.UpdateAsync(recordEntity);
                // 档案类型相同，进行编辑处理
                if (typeId == input.RecordType)
                {
                    var recordHistory = new RecordHistoryEntity()
                    {
                        RecordId = recordEntity.Id,
                        OperateType = "编辑",
                        OperateInfo = $"编辑了{recordEntity.RecordUserName}档案"
                    };
                    // 删除已移除的文件类别
                    var fileTypeIdList = fileInput.Where(i => i.CheckedRecordFileTypeId != 0).Select(i => i.CheckedRecordFileTypeId).ToList();
                    var deletedFileTypeIdList = await _checkedRecordFileTypeRepository.Select.Where(i => i.RecordId == input.Id).ToListAsync(i => i.Id);
                    foreach(var item in deletedFileTypeIdList.Except(fileTypeIdList))
                    {
                        await _checkedRecordFileTypeRepository.DeleteAsync(i => i.Id == item);
                        await _checkedRecordFileRepository.DeleteAsync(i => i.CheckedRecordFileTypeId == item);
                    }

                    foreach (var recordFileTypeInput in fileInput)
                    {
                        var recordFileType = await _recordFileTypeRepository.Select.WhereDynamic(recordFileTypeInput.RecordFileTypeId).ToOneAsync();
                        if (recordFileTypeInput.CheckedRecordFileTypeId == 0)
                        {
                           
                            var checkedFileList = recordFileTypeInput.Children.Where(i => i.Checked == true);

                            if (checkedFileList.Any())
                            {
                                var checkedRecordFileTypeEntity = new CheckedRecordFileTypeEntity()
                                {
                                    RecordFileTypeId = recordFileTypeInput.RecordFileTypeId,
                                    RecordId = recordEntity.Id,
                                    Remarks = recordFileTypeInput.Remarks
                                };
                                var checkedRecordFileType = await _checkedRecordFileTypeRepository.InsertAsync(checkedRecordFileTypeEntity);

                                recordHistory.OperateInfo += $"<br> 文件类型:{recordFileType.FileTypeName}";
                                if (!string.IsNullOrEmpty(checkedRecordFileType.Remarks))
                                {
                                    recordHistory.OperateInfo += $"-{checkedRecordFileType.Remarks}";
                                }

                                foreach (var item in checkedFileList)
                                {
                                    var entity = _mapper.Map<CheckedRecordFileEntity>(item);
                                    entity.CheckedRecordFileTypeId = checkedRecordFileType.Id;
                                    entity.RecordId = input.Id;
                                    await _checkedRecordFileRepository.InsertAsync(entity);
                                    if (item.OtherSign == 0)
                                    {
                                        recordHistory.OperateInfo += $"<br> 选中预设文件:{entity.Name} 份数:{entity.Num} 过期时间:{entity.CreditDueDate}";
                                    }
                                    else
                                    {
                                        recordHistory.OperateInfo += $"<br> 用户自定义文件:{entity.Name} 份数:{entity.Num} 过期时间:{entity.CreditDueDate}";
                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            var checkedRecordFileType = await _checkedRecordFileTypeRepository.Select.WhereDynamic(recordFileTypeInput.CheckedRecordFileTypeId).ToOneAsync();
                            var checkedFileList = recordFileTypeInput.Children.Where(i => i.Checked == true);

                            if (checkedFileList.Any() || checkedRecordFileType.Remarks != recordFileTypeInput.Remarks)
                            {
                                recordHistory.OperateInfo += $"<br> 文件类型:{recordFileType.FileTypeName}";
                                if (!string.IsNullOrEmpty(checkedRecordFileType.Remarks))
                                {
                                    recordHistory.OperateInfo += $"-{checkedRecordFileType.Remarks}";
                                }
                            }

                            //没有勾选项删除勾选文件类型
                            if(!checkedFileList.Any())
                            {
                                await _checkedRecordFileTypeRepository.DeleteAsync(checkedRecordFileType);
                            }

                            if (checkedRecordFileType.Remarks != recordFileTypeInput.Remarks)
                            {
                                recordHistory.OperateInfo += $"<br> 备注项更新  原备注项:{checkedRecordFileType.Remarks} 新备注项:{recordFileTypeInput.Remarks}";
                                checkedRecordFileType.Remarks = recordFileTypeInput.Remarks;
                                await _checkedRecordFileTypeRepository.UpdateAsync(checkedRecordFileType);
                            }
                            var oldCheckedRecordFileId = await _checkedRecordFileRepository.Select
                                .Where(i => i.RecordId == recordEntity.Id && i.CheckedRecordFileTypeId == recordFileTypeInput.CheckedRecordFileTypeId)
                                .ToListAsync(i => i.Id);

                            foreach (var item in checkedFileList)
                            {
                                if (oldCheckedRecordFileId.Contains(item.CheckedRecordFileId))
                                {
                                    var entity = await _checkedRecordFileRepository.Select.WhereDynamic(item.CheckedRecordFileId).ToOneAsync();
                                    if (item.Name != entity.Name || item.Num != entity.Num || item.CreditDueDate != entity.CreditDueDate)
                                    {
                                        if (entity.OtherSign == 0)
                                        {
                                            recordHistory.OperateInfo += $"<br> 选中项更新  原选中项: 名称:{entity.Name} 过期时间:{entity.CreditDueDate} 份数:{entity.Num} 新选中项: 名称:{item.Name} 过期时间:{item.CreditDueDate} 份数:{item.Num}";
                                        }
                                        else
                                        {
                                            recordHistory.OperateInfo += $"<br> 用户自定义项更新  原选中项: 名称:{entity.Name} 过期时间:{entity.CreditDueDate} 份数:{entity.Num} 新选中项: 名称:{item.Name} 过期时间:{item.CreditDueDate} 份数:{item.Num}";
                                        }
                                        _mapper.Map(item, entity);
                                        await _checkedRecordFileRepository.UpdateAsync(entity);
                                    }

                                    oldCheckedRecordFileId.Remove(item.CheckedRecordFileId);
                                }
                                else
                                {
                                    var entity = _mapper.Map<CheckedRecordFileEntity>(item);
                                    entity.CheckedRecordFileTypeId = recordFileTypeInput.CheckedRecordFileTypeId;
                                    entity.RecordId = recordEntity.Id;
                                    if (item.OtherSign == 0)
                                    {
                                        recordHistory.OperateInfo += $"<br> 选中预设文件:{entity.Name} 份数:{entity.Num} 过期时间:{entity.CreditDueDate}";
                                    }
                                    else
                                    {
                                        recordHistory.OperateInfo += $"<br> 用户自定义文件:{entity.Name} 份数:{entity.Num} 过期时间:{entity.CreditDueDate}";
                                    }
                                    await _checkedRecordFileRepository.InsertAsync(entity);
                                }
                            }
                            foreach (var item in oldCheckedRecordFileId)
                            {
                                var entity = await _checkedRecordFileRepository.Select.WhereDynamic(item).ToOneAsync();
                                if (entity.OtherSign == 0)
                                {
                                    recordHistory.OperateInfo += $"<br> 删除预设文件:{entity.Name} 份数:{entity.Num} 过期时间:{entity.CreditDueDate}";
                                }
                                else
                                {
                                    recordHistory.OperateInfo += $"<br> 删除用户自定义文件:{entity.Name} 份数:{entity.Num} 过期时间:{entity.CreditDueDate}";
                                }
                                await _checkedRecordFileRepository.DeleteAsync(item);
                            }
                        }
                    }

                    await _recordHistoryRepository.InsertAsync(recordHistory);
                }
                // 档案类型不同，删除原有类型的文件，进行新类型的插入工作
                else
                {
                    var recordHistory = new RecordHistoryEntity()
                    {
                        RecordId = recordEntity.Id,
                        OperateType = "编辑",
                        OperateInfo = $"编辑了{recordEntity.RecordUserName}档案 变更档案类型"
                    };

                    // 删除原有选中条目，其他文件和合同号
                    await _checkedRecordFileRepository.DeleteAsync(i => i.RecordId == recordEntity.Id);
                    await _checkedRecordFileTypeRepository.DeleteAsync(i => i.RecordId == recordEntity.Id);

                    // 插入新档案类型的文件
                    foreach (var recordFileTypeInput in fileInput)
                    {
                        var checkedRecordFileTypeEntity = new CheckedRecordFileTypeEntity()
                        {
                            RecordFileTypeId = recordFileTypeInput.RecordFileTypeId,
                            RecordId = recordEntity.Id,
                            Remarks = recordFileTypeInput.Remarks
                        };
                        var checkedRecordFileTypeId = await _checkedRecordFileTypeRepository.InsertAsync(checkedRecordFileTypeEntity);
                        var checkedFileList = recordFileTypeInput.Children.Where(i => i.Checked == true);

                        if (checkedFileList.Any())
                        {
                            var recordFileType = await _recordFileTypeRepository.Select.WhereDynamic(recordFileTypeInput.RecordFileTypeId).ToOneAsync();
                            recordHistory.OperateInfo += $"<br> 文件类型:{recordFileType.FileTypeName}";
                            if (!string.IsNullOrEmpty(checkedRecordFileTypeEntity.Remarks))
                            {
                                recordHistory.OperateInfo += $"-{checkedRecordFileTypeEntity.Remarks}";
                            }
                        }

                        foreach (var item in checkedFileList)
                        {
                            var entity = _mapper.Map<CheckedRecordFileEntity>(item);
                            entity.CheckedRecordFileTypeId = checkedRecordFileTypeId.Id;
                            entity.RecordId = recordEntity.Id;
                            if (entity.OtherSign == 0)
                            {
                                recordHistory.OperateInfo += $"<br> 选中预设文件:{entity.Name} 份数:{entity.Num} 过期时间:{entity.CreditDueDate}";
                            }
                            else
                            {
                                recordHistory.OperateInfo += $"<br> 用户自定义文件:{entity.Name} 份数:{entity.Num} 过期时间:{entity.CreditDueDate}";
                            }
                            await _checkedRecordFileRepository.InsertAsync(entity);
                        }
                    }

                    await _recordHistoryRepository.InsertAsync(recordHistory);
                }

                var noHandCount = await _checkedRecordFileRepository.Select
                    .Where(i => i.HandOverSign == 2 && i.RecordId == recordEntity.Id)
                    .CountAsync();
                if (noHandCount > 0)
                {
                    var handCount = await _checkedRecordFileRepository.Select
                        .Where(i => i.HandOverSign == 1 && i.RecordId == recordEntity.Id)
                        .CountAsync();
                    var borrowCount = await _checkedRecordFileRepository.Select
                        .Where(i => i.HandOverSign == 3 && i.RecordId == recordEntity.Id)
                        .CountAsync();
                    if (borrowCount > 0)
                    {
                        await _recordRepository.UpdateDiy.Set(i => i.Status, 2).Where(i => i.Id == recordEntity.Id).ExecuteAffrowsAsync();
                    }
                    else
                    {
                        if (handCount > 0)
                        {
                            await _recordRepository.UpdateDiy.Set(i => i.Status, 1).Where(i => i.Id == recordEntity.Id).ExecuteAffrowsAsync();
                        }
                        else
                        {
                            await _recordRepository.UpdateDiy.Set(i => i.Status, 0).Where(i => i.Id == recordEntity.Id).ExecuteAffrowsAsync();
                        }
                    }
                    await _checkedRecordFileRepository.UpdateDiy.Set(i => i.HandOverSign, 0).Where(i => i.RecordId == input.Id && i.HandOverSign == 2).ExecuteAffrowsAsync();
                }
                // await _checkedRecordFileRepository.UpdateDiy.Set(i => i.HandOverSign, 0).Where(i => i.RecordId == input.Id && i.HandOverSign == 2).ExecuteAffrowsAsync();

            }
            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> AddAdditionalRecordInfoAsync(RecordGetOutput record, List<RecordFileTypeAdditionalOutput> input)
        {
            var recordHistory = new RecordHistoryEntity()
            {
                RecordId = record.Id,
                OperateType = "补充提交",
                OperateInfo = $"补充提交 {record.UserName}档案"
            };
            // 删除已经删除的选中文件类别
            // 删除文件类型下有已移交的文件则只是删除未移交的文件，若全是未移交，则连同文件类型也一起删除
            var idList = input.Where(i => i.CheckedRecordFileTypeId != 0).Select(i => i.CheckedRecordFileTypeId).ToList();
            var oldIdList = await _checkedRecordFileTypeRepository.Select.Where(i => i.RecordId == record.Id).ToListAsync(i => i.Id);
            foreach(var checkedRecordFileTypeId in oldIdList.Except(idList))
            {
                var checkedFileCount = await _checkedRecordFileRepository.Select.Where(i => i.CheckedRecordFileTypeId == checkedRecordFileTypeId && i.HandOverSign != 0).CountAsync();
                // 删除未移交的文件
                if(checkedFileCount > 0)
                {
                    await _checkedRecordFileRepository.DeleteAsync(i => i.CheckedRecordFileTypeId == checkedRecordFileTypeId && i.HandOverSign == 0);
                }
                // 删除未移交的文件并且删除档案类别
                else
                {
                    await _checkedRecordFileTypeRepository.DeleteAsync(i => i.Id == checkedRecordFileTypeId);
                    await _checkedRecordFileRepository.DeleteAsync(i => i.CheckedRecordFileTypeId == checkedRecordFileTypeId && i.HandOverSign == 0);
                }
            }

            foreach (var recordFileType in input)
            {
                var child = recordFileType.Children.Where(i => i.Checked == true);

                if (!child.Any())
                {
                    if(recordFileType.CheckedRecordFileTypeId != 0)
                    {
                        // 判断当前文件类型下是否存在移交的文件，若无则删除当前文件类型，若有，则保留
                        var fileCount = await _checkedRecordFileRepository.Select.Where(i => i.CheckedRecordFileTypeId == recordFileType.CheckedRecordFileTypeId && i.HandOverSign != 0).CountAsync();
                        if(fileCount == 0)
                        {
                            await _checkedRecordFileTypeRepository.DeleteAsync(i => i.Id == recordFileType.CheckedRecordFileTypeId);
                        }
                        await _checkedRecordFileRepository.DeleteAsync(i => i.CheckedRecordFileTypeId == recordFileType.CheckedRecordFileTypeId && i.HandOverSign == 0);
                    }
                    // 对已经取消勾选文件至为0的文件类别进行删除
                    //if(recordFileType.CheckedRecordFileTypeId != 0)
                    //{
                    //    // 删除选中文件类别
                    //    await _checkedRecordFileTypeRepository.DeleteAsync(recordFileType.CheckedRecordFileTypeId);
                    //    // 删除选中文件
                    //    await _checkedRecordFileRepository.DeleteAsync(i => i.CheckedRecordFileTypeId == recordFileType.CheckedRecordFileTypeId);
                    //}
                }
                else
                {
                    var checkedRecordFileTypeId = recordFileType.CheckedRecordFileTypeId;
                    // 选中文件类别号为0代表是新增
                    if (recordFileType.CheckedRecordFileTypeId == 0)
                    {
                        var fileType = await _recordFileTypeRepository.Select.WhereDynamic(recordFileType.RecordFileTypeId).ToOneAsync();
                        var obj = _mapper.Map<CheckedRecordFileTypeEntity>(recordFileType);
                        obj.RecordId = record.Id;
                        var item = await _checkedRecordFileTypeRepository.InsertAsync(obj);
                        checkedRecordFileTypeId = item.Id;
                        recordHistory.OperateInfo += $"<br> 文件类型:{fileType.FileTypeName}";
                        if (!string.IsNullOrEmpty(obj.Remarks))
                        {
                            recordHistory.OperateInfo += $"-{obj.Remarks}";
                        }
                    }
                    var checkedRecordFileList = _mapper.Map<List<CheckedRecordFileEntity>>(child);
                    //var checkedRecordFileIdList = checkedRecordFileList.Select(i => i.Id).ToList();
                    //var dataBaseIdList = await _checkedRecordFileRepository.Select
                    //    .Where(i => i.RecordId == )
                    // 删除取消勾选的文件
                    var checkedFileIdList = checkedRecordFileList.Where(i => i.Id != 0).Select(i => i.Id);
                    // TODO 可能出现问题的地方，亟待解决
                    var oldCheckedFileIdList = await _checkedRecordFileRepository.Select.Where(i => i.CheckedRecordFileTypeId == checkedRecordFileTypeId && !checkedFileIdList.Contains(i.Id) && i.HandOverSign == 0).ToListAsync(i => i.Id);
                    await _checkedRecordFileRepository.DeleteAsync(i => oldCheckedFileIdList.Contains(i.Id));

                    foreach (var checkedRecordFile in checkedRecordFileList)
                    {
                        if (checkedRecordFile.OtherSign == 0)
                        {
                            // 未移交
                            if(checkedRecordFile.Id != 0)
                            {
                                var entity = await _checkedRecordFileRepository.Select.WhereDynamic(checkedRecordFile.Id).ToOneAsync();
                                entity.Num = checkedRecordFile.Num;
                                entity.CreditDueDate = checkedRecordFile.CreditDueDate;
                                await _checkedRecordFileRepository.UpdateAsync(entity);
                            }
                            else
                            {
                                // 查询是否有相同文件类型的未移交档案文件
                                var handsCheckedRecordFile = await _checkedRecordFileRepository.Select
                                    .Where(i => i.CheckedRecordFileTypeId == checkedRecordFileTypeId)
                                    .Where(i => i.RecordId == record.Id)
                                    .Where(i => i.RecordFileId == checkedRecordFile.RecordFileId)
                                    .Where(i => i.HandOverSign == 0)
                                    .ToOneAsync();
                                if (handsCheckedRecordFile != null)
                                {
                                    // 不为空则更新数目
                                    handsCheckedRecordFile.Num += checkedRecordFile.Num;
                                    await _checkedRecordFileRepository.UpdateAsync(handsCheckedRecordFile);
                                }
                                else
                                {
                                    // 为空则新插入
                                    checkedRecordFile.RecordId = record.Id;
                                    checkedRecordFile.CheckedRecordFileTypeId = checkedRecordFileTypeId;
                                    await _checkedRecordFileRepository.InsertAsync(checkedRecordFile);
                                }
                            }
                            
                            recordHistory.OperateInfo += $"<br> 选中预设文件:{checkedRecordFile.Name} 份数:{checkedRecordFile.Num} 过期时间:{checkedRecordFile.CreditDueDate}";
                        }
                        else
                        {
                            if (checkedRecordFile.Id != 0)
                            {
                                var entity = await _checkedRecordFileRepository.Select.WhereDynamic(checkedRecordFile.Id).ToOneAsync();
                                entity.Num = checkedRecordFile.Num;
                                entity.CreditDueDate = checkedRecordFile.CreditDueDate;
                                await _checkedRecordFileRepository.UpdateAsync(entity);
                            }
                            else
                            {
                                // 查询是否有相同文件类型的未移交档案文件
                                var handsCheckedRecordFile = await _checkedRecordFileRepository.Select
                                    .Where(i => i.CheckedRecordFileTypeId == checkedRecordFileTypeId)
                                    .Where(i => i.RecordFileId == 0 && i.HandOverSign == 0)
                                    .ToOneAsync();
                                if (handsCheckedRecordFile != null)
                                {
                                    // 不为空则更新数目
                                    handsCheckedRecordFile.Num += checkedRecordFile.Num;
                                    await _checkedRecordFileRepository.UpdateAsync(handsCheckedRecordFile);
                                }
                                else
                                {
                                    // 为空则新插入
                                    checkedRecordFile.RecordId = record.Id;
                                    checkedRecordFile.CheckedRecordFileTypeId = checkedRecordFileTypeId;
                                    await _checkedRecordFileRepository.InsertAsync(checkedRecordFile);
                                }
                            }
                            
                            recordHistory.OperateInfo += $"<br> 用户自定义文件:{checkedRecordFile.Name} 份数:{checkedRecordFile.Num} 过期时间:{checkedRecordFile.CreditDueDate}";
                        }
                    }
                }
            }

            // 更新当前档案状态
            var noHandCount = await _checkedRecordFileRepository.Select
                .Where(i => i.HandOverSign == 2 && i.RecordId == record.Id)
                .CountAsync();
            if(noHandCount > 0)
            {
                var handCount = await _checkedRecordFileRepository.Select
                    .Where(i => i.HandOverSign == 1 && i.RecordId == record.Id)
                    .CountAsync();
                var borrowCount = await _checkedRecordFileRepository.Select
                    .Where(i => i.HandOverSign == 3 && i.RecordId == record.Id)
                    .CountAsync();
                if (borrowCount > 0)
                {
                    await _recordRepository.UpdateDiy.Set(i => i.Status, 2).ExecuteAffrowsAsync();
                }
                else
                {
                    if(handCount > 0)
                    {
                        await _recordRepository.UpdateDiy.Set(i => i.Status, 1).ExecuteAffrowsAsync();
                    }
                    else
                    {
                        await _recordRepository.UpdateDiy.Set(i => i.Status, 0).ExecuteAffrowsAsync();
                    }
                }
                await _checkedRecordFileRepository.UpdateDiy.Set(i => i.HandOverSign, 0).Where(i => i.RecordId == record.Id && i.HandOverSign == 2).ExecuteAffrowsAsync();
            }
            
            await _recordHistoryRepository.InsertAsync(recordHistory);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> HandOverPageAsync(PageInput<RecordEntity> input)
        {
            //var entityList = _freeSql.Select<RecordEntity>().From<CheckedRecordFileEntity>((s, b) => s
            //    .LeftJoin(a => a.Id == b.RecordId))
            //    .Where((a, b) => b.HandOverSign == 0)
            //    .WhereIf(input.Filter.ManagerUserId.HasValue, (a, b) => a.ManagerUserId == input.Filter.ManagerUserId)
            //    .WhereIf(input.Filter.ManagerDepartmentId.HasValue, (a, b) => a.ManagerDepartmentId == input.Filter.ManagerDepartmentId)
            //    .Distinct()
            //    .Count(out var total)
            //    .Page(input.CurrentPage, input.PageSize)
            //    .ToList((a, b) => a);

            var entityList = await _recordRepository.Select
                .WhereIf(input.Filter.ManagerUserId.HasValue, a => a.ManagerUserId == input.Filter.ManagerUserId)
                .WhereIf(input.Filter.ManagerDepartmentId.HasValue, a => a.ManagerDepartmentId == input.Filter.ManagerDepartmentId)
                .Where(a => a.CheckedRecordFileList.AsSelect().Any(t => t.HandOverSign == 0))
                .Include(i => i.ManagerUser)
                .Include(i => i.ManagerDepartment)
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            //var checkedRecordFileIdList = await _checkedRecordFileRepository.Select
            //    .Where(i => i.HandOverSign == 0)
            //    .Distinct()
            //    .ToListAsync(i => i.RecordId);

            //var total = checkedRecordFileIdList.Count;
            //var list = checkedRecordFileIdList.Skip(input.CurrentPage == 1 ? 0 : (input.CurrentPage - 1) * input.PageSize).Take(input.PageSize);

            //var entityList = await _recordRepository.Select
            //    .Where(i => list.Contains(i.Id))
            //    .Include(i => i.ManagerUser)
            //    .Include(i => i.ManagerDepartment)
            //    //.Count(out var total)
            //    .OrderByDescending(i => i.CreatedTime)
            //    //.Page(input.CurrentPage, input.PageSize)
            //    .ToListAsync();

            var data = new PageOutput<HandOverRecordPageOutput>()
            {
                List = _mapper.Map<List<HandOverRecordPageOutput>>(entityList),
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        public IResponseOutput HandOverCheckAsync(HandOverBasicInfoOutput input)
        {
            _freeSql.Transaction(() =>
            {
                try
                {
                    var recordHistory = new RecordHistoryEntity()
                    {
                        RecordId = input.Record.Id,
                        OperateType = "档案移交",
                        OperateInfo = $"档案 {input.Record.RecordUserName} - {input.Record.RecordUserInCode} 进行移交操作"
                    };
                    var sign = false;

                    foreach (var item in input.RecordFileTypeList)
                    {
                        var count = item.Children.Where(i => i.Checked == true);

                        if (count.Any())
                        {
                            sign = true;
                            recordHistory.OperateInfo += $"<br> 档案类型 {item.Name}-{item.Remarks}";
                            foreach (var checkedRecordFile in count)
                            {
                                //var checkedRecordFileEntity = _checkedRecordFileRepository.Select.WhereDynamic(checkedRecordFile.Id).ToOne();
                                //var checkedRecordFileEntity = _freeSql.Select<CheckedRecordFileEntity>().WhereDynamic(checkedRecordFile.Id).ToOne();
                                // checkedRecordFileEntity.HandOverSign = 1;
                                
                                // _checkedRecordFileRepository.Update(checkedRecordFileEntity);
                                if (checkedRecordFile.OtherSign == 1)
                                {
                                    var entity = _freeSql.Select<CheckedRecordFileEntity>().Where(i => i.Id == checkedRecordFile.Id).ToOne();
                                    var handOverEntity = _freeSql.Select<CheckedRecordFileEntity>()
                                        .Where(i => i.RecordFileId == 0)
                                        .Where(i => i.Name == entity.Name)
                                        .Where(i => i.HandOverSign == 1).ToOne();
                                    if (handOverEntity != null)
                                    {
                                        handOverEntity.Num += checkedRecordFile.Num;
                                        _freeSql.Update<CheckedRecordFileEntity>().Set(i => i.Num, handOverEntity.Num).Where(i => i.Id == handOverEntity.Id).ExecuteAffrows();
                                        _freeSql.Delete<CheckedRecordFileEntity>().Where(i => i.Id == checkedRecordFile.Id).ExecuteAffrows();
                                    }
                                    else
                                    {
                                        _freeSql.Update<CheckedRecordFileEntity>().Set(i => i.HandOverSign, 1).Where(i => i.Id == checkedRecordFile.Id).ExecuteAffrows();
                                    }
                                    recordHistory.OperateInfo += $"<br> 自定义文件 {checkedRecordFile.Name} 过期时间:{checkedRecordFile.CreditDueDate} 份数:{checkedRecordFile.Num}";
                                }
                                else
                                {
                                    var entity = _freeSql.Select<CheckedRecordFileEntity>().Where(i => i.Id == checkedRecordFile.Id).ToOne();
                                    var handOverEntity = _freeSql.Select<CheckedRecordFileEntity>()
                                    .Where(i => i.CheckedRecordFileTypeId == entity.CheckedRecordFileTypeId)
                                    .Where(i => i.RecordFileId == entity.RecordFileId)
                                    .Where(i => i.HandOverSign == 1).ToOne();
                                    if (handOverEntity != null)
                                    {
                                        handOverEntity.Num += checkedRecordFile.Num;
                                        _freeSql.Update<CheckedRecordFileEntity>().Set(i => i.Num, handOverEntity.Num).Where(i => i.Id == handOverEntity.Id).ExecuteAffrows();
                                        _freeSql.Delete<CheckedRecordFileEntity>().Where(i => i.Id == checkedRecordFile.Id).ExecuteAffrows();
                                    }
                                    else
                                    {
                                        _freeSql.Update<CheckedRecordFileEntity>().Set(i => i.HandOverSign, 1).Where(i => i.Id == checkedRecordFile.Id).ExecuteAffrows();
                                    }
                                    recordHistory.OperateInfo += $"<br> 预设文件 {checkedRecordFile.Name} 过期时间:{checkedRecordFile.CreditDueDate} 份数:{checkedRecordFile.Num}";
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (sign)
                    {
                        //var recordEntity = _recordRepository.Select.WhereDynamic(input.Record.Id).ToOne();
                        //recordEntity.Status = 1;
                        //_recordRepository.Update(recordEntity);

                        //var recordEntity = _freeSql.Select<RecordEntity>().WhereDynamic(input.Record.Id).ToOne();
                        //recordEntity.Status = 1;

                        var record = _freeSql.Select<RecordEntity>().Where(i => i.Id == input.Record.Id).ToOne();
                        if(record.Status != 2)
                        {
                            _freeSql.Update<RecordEntity>().Set(i => i.Status, 1).Where(i => i.Id == input.Record.Id).ExecuteAffrows();
                        }
                    }

                    _freeSql.Insert(recordHistory).ExecuteAffrows();

                    //var entity = new NotifyEntity()
                    //{
                    //    UserId = input.Record.ManagerUserId.Value,
                    //    Message = $"{input.Record.RecordId}档案移交成功"
                    //};

                    //await _notifyRepository.InsertAsync(entity);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetHandOverInfoAsync(long id)
        {
            var output = new HandOverBasicInfoOutput();
            var entity = await _recordRepository.Select
                .WhereDynamic(id)
                .Include(a => a.ManagerDepartment)
                .Include(a => a.ManagerUser)
                .ToOneAsync();

            output.Record = _mapper.Map<RecordGetOutput>(entity);

            var checkedRecordFileTypeIdList = await _checkedRecordFileRepository.Select
                .Where(i => i.RecordId == id && i.HandOverSign == 0)
                .Distinct()
                .ToListAsync(i => i.CheckedRecordFileTypeId);

            var checkedRecordFileTypeList = await _checkedRecordFileTypeRepository.Select
                .Where(i => checkedRecordFileTypeIdList.Contains(i.Id))
                .Include(i => i.RecordFileType)
                .ToListAsync(i => new RecordFileTypeOutput() { RecordFileTypeId = i.RecordFileType.Id, Name = i.RecordFileType.FileTypeName, Remarks = i.Remarks, CheckedRecordFileTypeId = i.Id });


            output.RecordFileTypeList = checkedRecordFileTypeList;

            foreach (var item in output.RecordFileTypeList)
            {
                var checkedRecordFileList = await _checkedRecordFileRepository.Select
                .Where(i => i.RecordId == id && i.HandOverSign == 0 && i.CheckedRecordFileTypeId == item.CheckedRecordFileTypeId)
                .ToListAsync();

                if (checkedRecordFileList != null)
                {
                    item.Children = _mapper.Map<List<CheckedRecordFileInput>>(checkedRecordFileList);
                    foreach(var recordFile in item.Children)
                    {
                        recordFile.Checked = true;
                    }
                }
            }

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> GetListByUserAsync(long id)
        {
            var list = await _recordRepository.Select
                .Where(i => i.ManagerUserId == id)
                .OrderByDescending(i => i.CreatedTime)
                .ToListAsync(i => new { id = i.Id, UserName = i.RecordUserName, RecordId = i.RecordId });

            return ResponseOutput.Ok(list);
        }

        [Transaction]
        public async Task<IResponseOutput> RelationChangeAsync(List<RecordTransferInput> input)
        {
            
            foreach(var item in input)
            {
                var newUse = await _userRepository.Select.WhereDynamic(item.Value).ToOneAsync();
                foreach (var recordId in item.SelectRecord)
                {
                    var record = await _recordRepository.Select.WhereDynamic(recordId).Include(i => i.ManagerUser).ToOneAsync();
                    var recordHistory = new RecordHistoryEntity()
                    {
                        RecordId = recordId,
                        OperateType = "档案转让",
                        OperateInfo = $"档案{record.RecordUserName}-{record.RecordId}归属关系变更"
                    };

                    recordHistory.OperateInfo += $"<br> 原归属人:{record.ManagerUser.NickName}-{record.ManagerUser.UserName}";
                    recordHistory.OperateInfo += $"<br> 新归属人:{newUse.NickName}-{newUse.UserName}";

                    await _recordRepository.UpdateDiy.Set(i => i.ManagerUserId, item.Value).Where(i => i.Id == recordId).ExecuteAffrowsAsync();
                    await _recordHistoryRepository.InsertAsync(recordHistory);
                }
            }
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetPrintInfoAsync(long id)
        {
            var output = new RecordBasicInfoOutput();
            var entity = await _recordRepository.Select
                .WhereDynamic(id)
                .Include(a => a.ManagerDepartment)
                .Include(a => a.ManagerUser)
                .ToOneAsync();

            output.Record = _mapper.Map<RecordGetOutput>(entity);

            // 未移交打印版
            var entityList = await _checkedRecordFileTypeRepository.Select.As("k")
                .Where(i => i.CheckedRecordFileList.AsSelect().Any(a => a.HandOverSign == 0))
                .Where(i => i.RecordId == id)
                .ToListAsync(i => new RecordFileTypeOutput() { RecordFileTypeId = i.RecordFileTypeId, Name = i.RecordFileType.FileTypeName, Remarks = i.Remarks, CheckedRecordFileTypeId = i.Id });

            // 打印完整版
            //var entityList = await _checkedRecordFileTypeRepository.Select
            //    .Where(i => i.RecordId == id)
            //    .Include(i => i.RecordFileType)
            //    .ToListAsync(i => new RecordFileTypeOutput() { RecordFileTypeId = i.RecordFileTypeId, Name = i.RecordFileType.FileTypeName, Remarks = i.Remarks, CheckedRecordFileTypeId = i.Id });

            //var entityList = await _recordFileTypeRepository.Select.From<CheckedRecordFileTypeEntity>((s, b) => s.LeftJoin(a => a.Id == b.RecordFileTypeId))
            //    .Where((s, b) => s.RecordTypeId == entity.RecordType)
            //    .ToListAsync((s, b) => new RecordFileTypeOutput() { RecordFileTypeId = s.Id, Name = s.FileTypeName, Remarks = b.Remarks, CheckedRecordFileTypeId = b.Id });

            output.RecordFileTypeList = entityList;

            // 未移交打印版
            foreach (var item in output.RecordFileTypeList)
            {
                var recordFileList = await _recordFileRepository.Select
                    .Where(i => i.RecordFileTypeId == item.RecordFileTypeId)
                    .ToListAsync(i => new CheckedRecordFileInput() { Id = i.Id, Name = i.RecordFileName, RecordFileId = i.Id });

                var checkedRecordFileList = await _checkedRecordFileRepository.Select
                    .Where(i => i.CheckedRecordFileTypeId == item.CheckedRecordFileTypeId && i.HandOverSign == 0)
                    .ToListAsync();

                var res = _mapper.Map<List<CheckedRecordFileInput>>(checkedRecordFileList);

                foreach (var recordFile in recordFileList)
                {
                    foreach (var checkedRecordFile in res)
                    {
                        if (recordFile.Id == checkedRecordFile.RecordFileId)
                        {
                            recordFile.Checked = true;
                            recordFile.Num = checkedRecordFile.Num;
                        }
                    }
                }

                var other = res.Where(i => i.OtherSign == 1 && i.HandOverSign == 0);
                foreach (var oth in other)
                {
                    var obj = new CheckedRecordFileInput()
                    {
                        Id = oth.Id,
                        Name = oth.Name,
                        Num = oth.Num,
                        Checked = true
                    };
                    recordFileList.Add(obj);
                }

                item.Children = recordFileList;
            }

            // 打印完整版
            //foreach (var item in output.RecordFileTypeList)
            //{
            //    var recordFileList = await _recordFileRepository.Select
            //        .Where(i => i.RecordFileTypeId == item.RecordFileTypeId)
            //        .ToListAsync(i => new CheckedRecordFileInput() { Id = i.Id, Name = i.RecordFileName, RecordFileId = i.Id });

            //    var checkedRecordFileType = await _checkedRecordFileTypeRepository.Select
            //        .Where(i => i.RecordId == entity.Id && i.RecordFileTypeId == item.RecordFileTypeId && i.Id == item.CheckedRecordFileTypeId)
            //        .IncludeMany(i => i.CheckedRecordFileList, then => then.Include(a => a.RecordFile))
            //        .ToOneAsync();

            //    var res = _mapper.Map<List<CheckedRecordFileInput>>(checkedRecordFileType.CheckedRecordFileList);

            //    foreach(var recordFile in recordFileList)
            //    {
            //        foreach(var checkedRecordFile in res)
            //        {
            //            if(recordFile.Id == checkedRecordFile.RecordFileId)
            //            {
            //                recordFile.Checked = true;
            //                recordFile.Num = checkedRecordFile.Num;
            //            }
            //        }
            //    }

            //    var other = res.Where(i => i.OtherSign == 1);
            //    foreach(var oth in other)
            //    {
            //        var obj = new CheckedRecordFileInput()
            //        {
            //            Id = oth.Id,
            //            Name = oth.Name,
            //            Num = oth.Num,
            //            Checked = true
            //        };
            //        recordFileList.Add(obj);
            //    }

            //    item.Children = recordFileList;
            //}

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> GetExpiredRecordListAsync(PageInput<RecordEntity> input)
        {
            var recordIdList = await _checkedRecordFileRepository.Select
                .Where(i => i.CreditDueDate != null && i.CreditDueDate < DateTime.Now && i.HandOverSign == 1)
                .Distinct()
                .ToListAsync(i => i.RecordId);

            var recordList = await _recordRepository.Select
                .WhereIf(input.Filter.ManagerUserId != null, i => i.ManagerUserId == input.Filter.ManagerUserId)
                .WhereIf(input.Filter.ManagerDepartmentId != null, i => i.ManagerDepartmentId == input.Filter.ManagerDepartmentId)
                .Where(i => recordIdList.Contains(i.Id))
                .Include(i => i.ManagerUser)
                .Include(i => i.ManagerDepartment)
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var data = new PageOutput<RecordListOutput>()
            {
                List = _mapper.Map<List<RecordListOutput>>(recordList),
                Total = total
            };

            return ResponseOutput.Ok(data);

        }

        public async Task<IResponseOutput> GetChangeDetailAsync(long id)
        {
            var checkedRecordFileList = await _checkedRecordFileRepository.Select
                .Where(i => i.CreditDueDate != null && i.CreditDueDate < DateTime.Now && i.RecordId == id)
                .ToListAsync();

            var checkedRecordFileTypeIdList = checkedRecordFileList.Select(i => i.CheckedRecordFileTypeId).Distinct();

            var checkedRecordFileTypeList = await _checkedRecordFileTypeRepository.Select
                .Where(i => checkedRecordFileTypeIdList.Contains(i.Id))
                .Include(i => i.RecordFileType)
                .ToListAsync(i => new RecordFileTypeOutput() { RecordFileTypeId = i.RecordFileType.Id, Name = i.RecordFileType.FileTypeName, Remarks = i.Remarks, CheckedRecordFileTypeId = i.Id });


            foreach (var item in checkedRecordFileTypeList)
            {
                item.Children = _mapper.Map<List<CheckedRecordFileInput>>(checkedRecordFileList.Where(i => i.CheckedRecordFileTypeId == item.CheckedRecordFileTypeId));
            }


            return ResponseOutput.Ok(checkedRecordFileTypeList);
        }

        [Transaction]
        public async Task<IResponseOutput> AppleChangeFileAsync(int type, List<RecordFileTypeOutput> input)
        {
            var record = await _checkedRecordFileTypeRepository.Select.WhereDynamic(input[0].CheckedRecordFileTypeId).Include(i => i.Record).ToOneAsync(i => i.Record);
            var initiativaUpdate = new InitiativeUpdateEntity()
            {
                RecordId = record.Id,
                Type = type
            };

            var insert = await _initiativeUpdateRepository.InsertAsync(initiativaUpdate);
            foreach(var item in input)
            {
                foreach(var index in item.Children)
                {
                    var obj = _mapper.Map<InitiativeUpdateItemEntity>(index);
                    obj.InitiativeUpdateId = insert.Id;
                    await _initiativeUpdateItemRepository.InsertAsync(obj);
                }
            }

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetApplyChangeListAsync(PageInput<InitiativeUpdateEntity> input)
        {
            var entityList = await _initiativeUpdateRepository.Select
                .Where(i => i.Status == 0)
                .Include(i => i.Record)
                .Include(i => i.ApplyUser)
                .IncludeMany(i => i.ApplyUser.Departments)
                .Count(out var total)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync();

            var output = new PageOutput<InitiativeListOutput>()
            {
                Total = total,
                List = _mapper.Map<List<InitiativeListOutput>>(entityList)
            };

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> GetApplyChangeDetailAsync(long id)
        {
            var initiativeUpdateItemList = await _initiativeUpdateItemRepository.Select
                .Where(i => i.InitiativeUpdateId == id)
                .Include(i => i.CheckedRecordFile)
                .ToListAsync(i => i.CheckedRecordFile);

            var idList = initiativeUpdateItemList.Select(i => i.CheckedRecordFileTypeId).Distinct();

            var checkedRecordFileTypeList = await _checkedRecordFileTypeRepository.Select
                .Where(i => idList.Contains(i.Id))
                .Include(i => i.RecordFileType)
                .ToListAsync(i => new ChangeFileRecordFileTypeOutput() { CheckedRecordFileTypeId = i.Id, Name = i.RecordFileType.FileTypeName, Remarks = i.Remarks });

            foreach(var item in checkedRecordFileTypeList)
            {
                var list = await _initiativeUpdateItemRepository.Select
                    .Where(i => i.InitiativeUpdateId == id && i.CheckedRecordFile.CheckedRecordFileTypeId == item.CheckedRecordFileTypeId)
                    .ToListAsync();

                item.Children = _mapper.Map<List<InitiativeUpdateItemOutput>>(list);
            }

            return ResponseOutput.Ok(checkedRecordFileTypeList);
        }

        [Transaction]
        public async Task<IResponseOutput> AcceptApplyChangeAsync(long id)
        {
            await _initiativeUpdateRepository.UpdateDiy.Set(i => i.Status, 1).WhereDynamic(id).ExecuteAffrowsAsync();

            var record = await _initiativeUpdateRepository.Select
                .WhereDynamic(id)
                .Include(i => i.Record)
                .ToOneAsync(i => i.Record);
            var initiativeUpdateItemList = await _initiativeUpdateItemRepository.Select
                .Where(i => i.InitiativeUpdateId == id)
                .Include(i => i.CheckedRecordFile)
                .ToListAsync();

            foreach(var item in initiativeUpdateItemList)
            {
                if (item.DelSign)
                {
                    await _checkedRecordFileRepository.DeleteAsync(item.CheckedRecordFile);
                }
                else
                {
                    item.CheckedRecordFile.CreditDueDate = item.CreditDueDate;
                    await _checkedRecordFileRepository.UpdateAsync(item.CheckedRecordFile);
                }
            }

            var entity = new NotifyEntity()
            {
                UserId = record.ManagerUserId.GetValueOrDefault(),
                Message = $"{record.RecordId}档案申请更换文件成功"
            };

            await _notifyRepository.InsertAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> RefuseApplyChangeAsync(long id, string refuseReason)
        {
            await _initiativeUpdateRepository.UpdateDiy.Set(i => i.Status, 2).Set(i => i.RefuseReason, refuseReason).WhereDynamic(id).ExecuteAffrowsAsync();

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> GetNeedCreateRecordList(int type, string userCode, long departmentCode, PageInput<NeedCreateRecordEntity> input)
        {
            var department = await _departmentRepository.Select.WhereDynamic(departmentCode).ToOneAsync();
            string sqlStr;
            string countStr;
            var begin = input.CurrentPage - 1 == 0 ? 1 : (input.CurrentPage - 1) * input.PageSize;
            var end = input.CurrentPage * input.PageSize - 1;
            //type=0 管理员 type=1 会计主管 type=2 客户经理
            if (type == 0)
            {
                sqlStr = $"select ContractNo, CustINNO, CustNO, Custname from" +
                    $" (select a.ContractNo, a.CustINNO, a.CustNO, a.Custname, ROW_NUMBER() OVER(order by a.ContractNo) as idx" +
                    $" from rightcontrol.dbo.ZH_XDLZ_LoanContractNeedCreate a" +
                    $" inner join rightcontrol.dbo.ZH_XDLZ_LoanCustOfEmp b" +
                    $" on a.ContractNo=b.ContractNo" +
                    $" where b.Data_date='{DateTime.Now.AddDays(-1).ToShortDateString()}') as a" +
                    $" where idx between {begin} and {end}";
                countStr = $"select COUNT(*)" +
                    $" from rightcontrol.dbo.ZH_XDLZ_LoanContractNeedCreate a" +
                    $" inner join rightcontrol.dbo.ZH_XDLZ_LoanCustOfEmp b" +
                    $" on a.ContractNo=b.ContractNo" +
                    $" where b.Data_date='{DateTime.Now.AddDays(-1).ToShortDateString()}'";
            }
            else if (type == 1)
            {
                sqlStr = $"select ContractNo, CustINNO, CustNO, Custname from" +
                    $" (select a.ContractNo, a.CustINNO, a.CustNO, a.Custname, ROW_NUMBER() OVER(order by a.ContractNo) as idx" +
                    $" from rightcontrol.dbo.ZH_XDLZ_LoanContractNeedCreate a" +
                    $" inner join rightcontrol.dbo.ZH_XDLZ_LoanCustOfEmp b" +
                    $" on a.ContractNo=b.ContractNo" +
                    $" where b.Data_date='{DateTime.Now.AddDays(-1).ToShortDateString()}' and b.Instcode='{department.DepartmentCode}') as a" +
                    $" where idx between {begin} and {end}";
                countStr = $"select COUNT(*)" +
                    $" from rightcontrol.dbo.ZH_XDLZ_LoanContractNeedCreate a" +
                    $" inner join rightcontrol.dbo.ZH_XDLZ_LoanCustOfEmp b" +
                    $" on a.ContractNo=b.ContractNo" +
                    $" where b.Data_date='{DateTime.Now.AddDays(-1).ToShortDateString()}' and b.Instcode='{department.DepartmentCode}'";
            }
            else
            {
                sqlStr = $"select ContractNo, CustINNO, CustNO, Custname from" +
                    $" (select a.ContractNo, a.CustINNO, a.CustNO, a.Custname, ROW_NUMBER() OVER(order by a.ContractNo) as idx" +
                    $" from rightcontrol.dbo.ZH_XDLZ_LoanContractNeedCreate a" +
                    $" inner join rightcontrol.dbo.ZH_XDLZ_LoanCustOfEmp b" +
                    $" on a.ContractNo=b.ContractNo" +
                    $" where b.Data_date='{DateTime.Now.AddDays(-1).ToShortDateString()}' and (b.Tlrno1='{userCode}' or b.Tlrno2='{userCode}' or b.Tlrno3='{userCode}')) as a" +
                    $" where idx between {begin} and {end}";
                countStr = $"select COUNT(*)" +
                    $" from rightcontrol.dbo.ZH_XDLZ_LoanContractNeedCreate a" +
                    $" inner join rightcontrol.dbo.ZH_XDLZ_LoanCustOfEmp b" +
                    $" on a.ContractNo=b.ContractNo" +
                    $" where b.Data_date='{DateTime.Now.AddDays(-1).ToShortDateString()}' and (b.Tlrno1='{userCode}' or b.Tlrno2='{userCode}' or b.Tlrno3='{userCode}')";
            }

            var result = await _freeSql.Ado.QueryAsync<NeedCreateRecordEntity>(sqlStr);
            var count = await _freeSql.Ado.QueryAsync<long>(countStr);
            //var result = new List<NeedCreateRecordEntity>();
            //result.Add(new NeedCreateRecordEntity()
            //{
            //    CustINNO = "82800000",
            //    Custname = "方勇",
            //    ContractNo = "828123456",
            //    CustNO = "999999"
            //});
            //var count = 1;
            //var count = await _freeSql.Ado.QueryAsync<long>($"select count(*) from rt_departmentseed");

            var data = new PageOutput<NeedCreateRecordEntity>()
            {
                List = result,
                Total = count.FirstOrDefault()
            };

            return ResponseOutput.Ok(data);
        }

        [Transaction]
        public async Task<IResponseOutput> StockAddAsync(RecordAddInput input, List<RecordFileTypeOutput> fileInput)
        {
            var entity = _mapper.Map<RecordEntity>(input);

            var existEntity = await _recordRepository.Select
                .Where(i => i.RecordUserCode == entity.RecordUserCode || i.RecordUserInCode == entity.RecordUserInCode)
                .Where(i => i.ManagerDepartmentId == entity.ManagerDepartmentId)
                .Count(out var count)
                .ToOneAsync();

            if (count != 0)
            {
                return ResponseOutput.NotOk("同个支行下存在相同客户内码或者客户号的客户，不允许重复添加!");
            }

            var ins = new RecordIdEntity()
            {
                msg = "获取id"
            };

            var departmentEntity = await _departmentRepository.Select.WhereDynamic(input.ManagerDepartmentId).ToOneAsync();
            var redisKey = Enum.GetName(typeof(DepartmentCode), departmentEntity.DepartmentCode);
            var seedValue = _redisDataHelper.IncrIndex(redisKey);
            var recordId = $"AAAA{departmentEntity.DepartmentCode}{seedValue.ToString().PadLeft(5, '0')}";

            //var recordIdEntity = await _recordIdRepository.InsertAsync(ins);
            //var department = await _departmentRepository.Select.WhereDynamic(entity.ManagerDepartmentId).ToOneAsync();
            //var recordId = $"AAAA{department.DepartmentCode}{recordIdEntity.Id.ToString().PadLeft(5, '0')}";
            entity.RecordId = recordId;
            var record = await _recordRepository.InsertAsync(entity);

            if (!(record?.Id > 0))
            {
                return ResponseOutput.NotOk("发生错误，请刷新后重试");
            }
            else
            {
                var recordHistory = new RecordHistoryEntity()
                {
                    RecordId = record.Id,
                    OperateType = "新增",
                    OperateInfo = $"创建了{record.RecordUserName}档案"
                };
                foreach (var recordFileTypeInput in fileInput)
                {
                    var recordFileList = recordFileTypeInput.Children.Where(i => i.Checked == true).ToList();
                    if (recordFileList.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        var checkedRecordFileTypeEntity = _mapper.Map<CheckedRecordFileTypeEntity>(recordFileTypeInput);
                        checkedRecordFileTypeEntity.RecordId = record.Id;
                        var checkedRecordFileType = await _checkedRecordFileTypeRepository.InsertAsync(checkedRecordFileTypeEntity);

                        if (!(checkedRecordFileType.Id > 0))
                        {
                            return ResponseOutput.NotOk();
                        }
                        // 档案文件处理
                        foreach (var checkedRecordFile in recordFileTypeInput.Children.Where(i => i.Checked == true).ToList())
                        {
                            var checkedRecordFileEntity = _mapper.Map<CheckedRecordFileEntity>(checkedRecordFile);
                            checkedRecordFileEntity.RecordId = record.Id;
                            checkedRecordFileEntity.CheckedRecordFileTypeId = checkedRecordFileType.Id;
                            var returnModel = await _checkedRecordFileRepository.InsertAsync(checkedRecordFileEntity);
                            var recordFile = await _recordFileRepository.GetAsync(returnModel.RecordFileId);
                            if (checkedRecordFileEntity.OtherSign == 0)
                            {
                                recordHistory.OperateInfo += $"<br> 选中档案文件:{recordFileTypeInput.Name}-{recordFile.RecordFileName} 份数:{returnModel.Num}";
                            }
                            else if (checkedRecordFileEntity.OtherSign == 1)
                            {
                                recordHistory.OperateInfo += $"<br> 用户自定义文件:{checkedRecordFileEntity.Name} 份数:{returnModel.Num}";
                            }
                        }
                    }
                }
                await _recordHistoryRepository.InsertAsync(recordHistory);
            }

            await _freeSql.Ado.ExecuteNonQueryAsync($"delete from rightcontrol.dbo.ZH_XDLZ_LoanContractNeedCreate where CustINNO='{record.RecordUserInCode}'");

            return ResponseOutput.Ok();
        }

        public async Task<RecordEntity> GetRecordByIniId(long id)
        {
            var entity = await _initiativeUpdateRepository.Select
                .WhereDynamic(id)
                .Include(i => i.Record)
                .ToOneAsync(i => i.Record);

            return entity;
        }

        public async Task<IResponseOutput> ChangeRecordStatusAsync(int status, long id)
        {
            await _recordRepository.UpdateDiy.Set(i => i.Status, status).Where(i => i.Id == id).ExecuteAffrowsAsync();

            return ResponseOutput.Ok();
        }
    }
}
