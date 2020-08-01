using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Department;
using Admin.Core.Service.Admin.Department.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Admin.Core.Controllers.Admin
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class DepartmentController : AreaController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetList(string key)
        {
            var result = await _departmentService.ListAsync(key);
            return result;
        }

        /// <summary>
        /// 获取单个部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetDepartment(long id)
        {
            return await _departmentService.GetAsync(id);
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> UpdateDepartment(DepartmentUpdateInput input)
        {
            return await _departmentService.UpdateAsync(input);
        }

        /// <summary>
        /// 增加部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddDepartment(DepartmentUpdateInput input)
        {
            return await _departmentService.AddAsync(input);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _departmentService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> DeleteDepartment(long id)
        {
            return await _departmentService.DeleteAsync(id);
        }
    }
}