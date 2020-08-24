using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin.Department;
using Admin.Core.Service.Admin.Department.Input;
using Admin.Core.Service.Admin.Department.Output;
using AutoMapper;

namespace Admin.Core.Service.Admin.Department
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(
            IMapper mapper,
            IDepartmentRepository DepartmentRepository
        )
        {
            _mapper = mapper;
            _departmentRepository = DepartmentRepository;
        }

        public async Task<IResponseOutput> ListAsync(string key)
        {
            var data = await _departmentRepository
                .WhereIf(key.NotNull(), a => a.DepartmentName.Contains(key) || a.DepartmentCode.ToString().Contains(key))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Id)
                .ToListAsync<DepartmentListOutput>();

            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> GetAsync(long id)
        {
            var result = await _departmentRepository.GetAsync<DepartmentListOutput>(id);

            return ResponseOutput.Ok(result);
        }

        public async Task<IResponseOutput> UpdateAsync(DepartmentUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _departmentRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                return ResponseOutput.NotOk("获取当前编辑部门失败!");
            }

            _mapper.Map(input, entity);
            await _departmentRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> AddAsync(DepartmentUpdateInput input)
        {
            var exist_sign = _departmentRepository.Select.Any(a => a.DepartmentCode == input.DepartmentCode);
            if (!exist_sign)
            {
                var entity = _mapper.Map<DepartmentEntity>(input);
                var id = (await _departmentRepository.InsertAsync(entity)).Id;

                return ResponseOutput.Result(id > 0);
            }
            else
            {
                return ResponseOutput.NotOk("部门编号已经存在!");
            }
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _departmentRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _departmentRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> GetSelectListAsync(long id)
        {
            var result = await _departmentRepository.Select
                .WhereIf(id != 0, i => i.Id == id)
                .ToListAsync(i => new { id = i.Id, value = i.DepartmentName });

            return ResponseOutput.Ok(result);
        }
    }
}
