using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Record;
using Admin.Core.Repository.Record.CheckedRecordFile;
using Admin.Core.Repository.Record.CheckedRecordFileType;
using Admin.Core.Repository.Record.Record;
using Admin.Core.Repository.Record.RecordFile;
using Admin.Core.Repository.Record.RecordFileType;
using Admin.Core.Service.Record.RecordFile.Output;
using Admin.Core.Service.Record.RecordFileType.Input;
using Admin.Core.Service.Record.RecordFileType.Output;
using AutoMapper;

namespace Admin.Core.Service.Record.RecordFileType
{
    public class RecordFileTypeService:IRecordFileTypeService
    {
        private readonly IMapper _mapper;
        private readonly IRecordFileTypeRepository _recordFileTypeRepository;
        private readonly IRecordRepository _recordRepository;
        private readonly ICheckedRecordFileTypeRepository _checkedRecordFileTypeRepository;
        private readonly ICheckedRecordFileRepository _checkedRecordFileRepository;
        private readonly IRecordFileRepository _recordFileRepository;

        public RecordFileTypeService(IMapper mapper
            , IRecordFileTypeRepository recordFileTypeRepository
            , IRecordRepository recordRepository
            , ICheckedRecordFileTypeRepository checkedRecordFileTypeRepository
            , ICheckedRecordFileRepository checkedRecordFileRepository
            , IRecordFileRepository recordFileRepository)
        {
            _mapper = mapper;
            _recordFileTypeRepository = recordFileTypeRepository;
            _recordRepository = recordRepository;
            _checkedRecordFileTypeRepository = checkedRecordFileTypeRepository;
            _recordFileRepository = recordFileRepository;
            _checkedRecordFileRepository = checkedRecordFileRepository;
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var entity = await _recordFileTypeRepository.GetAsync(id);

            return ResponseOutput.Ok(entity);
        }

        public async Task<IResponseOutput> GetRecordFileListAsync(long id)
        {
            var entityList = await _recordFileTypeRepository.Select
                .Where(i => i.RecordTypeId == id)
                .ToListAsync();

            var output = _mapper.Map<List<RecordFileTypeListOutput>>(entityList);

            return ResponseOutput.Ok(output);
        }

        public async Task<IResponseOutput> AddRecordFileTypeAsync(RecordFileTypeAddInput input)
        {
            var entity = _mapper.Map<RecordFileTypeEntity>(input);

            var addSign = (await _recordFileTypeRepository.InsertAsync(entity)).Id > 0;
            return ResponseOutput.Result(addSign);
        }

        public async Task<IResponseOutput> UpdateRecordFileTypeAsync(RecordFileTypeUpdateInput input)
        {
            var entity = await _recordFileTypeRepository.GetAsync(input.Id);

            if(!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("档案更新的档案文件类型不存在");
            }
            else
            {
                _mapper.Map(input, entity);
                await _recordFileTypeRepository.UpdateAsync(entity);

                return ResponseOutput.Ok();
            }
        }

        public async Task<IResponseOutput> DeleteRecordFileTypeAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _recordFileTypeRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }


        public async Task<List<RecordFileTypeUpdateOutput>> UpdateRecordPageListAsync(long id, long recordId)
        {
            var entity = await _recordRepository.Select.WhereDynamic(recordId).ToOneAsync();

            var entityList = await _recordFileTypeRepository.Select
                .Where(a => a.RecordTypeId == entity.RecordType)
                .ToListAsync(a => new RecordFileTypeUpdateOutput() { RecordFileTypeId = a.Id, Name = a.FileTypeName });

            var checkedRecordFileTypeList = await _checkedRecordFileTypeRepository.Select
                .Where(a => a.RecordId == recordId)
                .ToListAsync();

            for(var i = entityList.Count - 1; i >= 0; i--)
            {
                foreach(var item in checkedRecordFileTypeList)
                {
                    if(entityList[i].RecordFileTypeId == item.RecordFileTypeId)
                    {
                        if(entityList[i].CheckedRecordFileTypeId == 0)
                        {
                            entityList[i].CheckedRecordFileTypeId = item.Id;
                            entityList[i].Remarks = item.Remarks;
                        }
                        else
                        {
                            var obj = new RecordFileTypeUpdateOutput()
                            {
                                RecordFileTypeId = entityList[i].RecordFileTypeId,
                                Name = entityList[i].Name,
                                CheckedRecordFileTypeId = item.Id,
                                Remarks = item.Remarks
                            };
                            entityList.Insert(i + 1, obj);
                        }
                    }
                }   
            }

            foreach (var item in entityList)
            {
                var recordFileList = await _recordFileRepository.Select
                    .Where(i => i.RecordFileTypeId == item.RecordFileTypeId)
                    .ToListAsync(i => new RecordFileUpdateOutput { Name = i.RecordFileName, RecordFileId = i.Id });

                //var recordFileList = await _recordFileRepository.Select
                //    .From<CheckedRecordFileEntity>((s, b) => s.LeftJoin(a => a.Id == b.RecordFileId))
                //    .Where((s, b) => s.RecordFileTypeId == item.RecordFileTypeId && b.CheckedRecordFileTypeId == item.CheckedRecordFileTypeId).ToListAsync((s, b) => new RecordFileUpdateOutput { Name = s.RecordFileName, CheckedRecordFileId = b.Id, RecordFileId = s.Id });

                var checkedRecordFileType = await _checkedRecordFileTypeRepository.Select
                    .Where(i => i.RecordId == entity.Id && i.RecordFileTypeId == item.RecordFileTypeId && i.Id == item.CheckedRecordFileTypeId)
                    .IncludeMany(i => i.CheckedRecordFileList, then => then.Include(a => a.RecordFile))
                    .ToOneAsync();

                //item.Children = _mapper.Map<List<RecordFileUpdateOutput>>(recordFileList);
                item.Children = recordFileList;

                if(item.CheckedRecordFileTypeId == 0)
                {
                    continue;
                }
                else
                {
                    var checkedRecordFileList = await _checkedRecordFileRepository.Select
                        .Where(i => i.CheckedRecordFileTypeId == item.CheckedRecordFileTypeId)
                        .ToListAsync();

                    if(checkedRecordFileList.Where(i => i.OtherSign == 0).Count() > 0)
                    {
                        foreach (var i in item.Children)
                        {
                            foreach (var j in checkedRecordFileList.Where(i => i.OtherSign == 0))
                            {
                                if(i.RecordFileId == j.RecordFileId)
                                {
                                    i.CheckedRecordFileId = j.Id;
                                    i.Checked = true;
                                    i.CreditDueDate = j.CreditDueDate;
                                    i.Num = j.Num;
                                    i.OtherSign = j.OtherSign;
                                    i.HandOverSign = j.HandOverSign;
                                }
                            }
                        }
                    }
                    foreach (var i in checkedRecordFileList.Where(i => i.OtherSign == 1))
                    {
                        var obj = new RecordFileUpdateOutput()
                        {
                            CheckedRecordFileId = i.Id,
                            Name = i.Name,
                            Num = i.Num,
                            CreditDueDate = i.CreditDueDate,
                            Checked = true,
                            OtherSign = 1,
                            HandOverSign = i.HandOverSign
                        };
                        item.Children.Add(obj);
                    }

                }

                //if (checkedRecordFileType != null)
                //{
                //    item.Remarks = checkedRecordFileType.Remarks;
                //    foreach(var i in checkedRecordFileType.CheckedRecordFileList)
                //    {
                //        if (i.RecordFile == null)
                //        {
                //            var obj = new RecordFileUpdateOutput()
                //            {
                //                CheckedRecordFileId = i.Id,
                //                Name = i.Name,
                //                Num = i.Num,
                //                CreditDueDate = i.CreditDueDate,
                //                Checked = true,
                //                OtherSign = 1,
                //                HandOverSign = i.HandOverSign
                //            };
                //            item.Children.Add(obj);
                //        }
                //        else
                //        {
                //            foreach (var j in item.Children)
                //            {
                //                if (i.Id == j.CheckedRecordFileId)
                //                {
                //                    j.CheckedRecordFileId = i.Id;
                //                    j.Checked = true;
                //                    j.CreditDueDate = i.CreditDueDate;
                //                    j.Num = i.Num;
                //                    j.OtherSign = i.OtherSign;
                //                    j.HandOverSign = i.HandOverSign;
                //                }
                //                else
                //                {
                //                    continue;
                //                }
                //            }
                //        }
                //    }

                //    //foreach(var i in checkedRecordFileType.CheckedRecordFileList)
                //    //{
                //    //    if(i.RecordFile == null)
                //    //    {
                //    //        var obj = new RecordFileUpdateOutput()
                //    //        {
                //    //            CheckedRecordFileId = i.Id,
                //    //            Name = i.Name,
                //    //            Num = i.Num,
                //    //            CreditDueDate = i.CreditDueDate,
                //    //            Checked = true,
                //    //            OtherSign = 1,
                //    //            HandOverSign = i.HandOverSign
                //    //        };
                //    //        item.Children.Add(obj);
                //    //    }
                //    //}
                //}
            }

            return entityList;
        }

        public async Task<List<RecordFileTypeAddOutput>> AddRecordPageListAsync(long id)
        {
            var entityList = await _recordFileTypeRepository.Select
                .Where(i => i.RecordTypeId == id)
                .IncludeMany(i => i.RecordFileList)
                .ToListAsync();

            var output = _mapper.Map<List<RecordFileTypeAddOutput>>(entityList);

            return output;
        }
    }
}
