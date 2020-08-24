using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Hubs;
using Admin.Core.Model.Record;
using Admin.Core.Service.Admin.Department;
using Admin.Core.Service.Admin.User;
using Admin.Core.Service.Record.Record;
using Admin.Core.Service.Record.Record.Input;
using Admin.Core.Service.Record.Record.Output;
using Admin.Core.Service.Record.RecordFileType.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace Admin.Core.Controllers.Record
{
    public class RecordController:AreaController
    {
        private readonly IUserService _userService;
        private readonly IRecordService _recordService;
        private readonly IDepartmentService _departmentService;
        private readonly IUser _user;
        private readonly IHubContext<ChatHub> _hubContext;

        public RecordController(IUser user, IRecordService recordService, IUserService userService, IHubContext<ChatHub> hubContext, IDepartmentService departmentService)
        {
            _user = user;
            _recordService = recordService;
            _userService = userService;
            _hubContext = hubContext;
            _departmentService = departmentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Test()
        {
            await _hubContext.Clients.Group("Admin").SendAsync("Show", "测试文字");
            return ResponseOutput.Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordService.GetAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPage(PageInput<RecordEntity> input)
        {
            var permissionName = await _userService.GetPermissionsNameAsync();
            var user = await _userService.GetAsync(_user.Id);

            var fullAccess = permissionName.Any(m => m != null && m.Equals("档案完全访问"));
            var partialAccess = permissionName.Any(m => m != null && m.Equals("档案所属部门访问"));
            var limitedAccess = permissionName.Any(m => m != null && m.Equals("档案所属经理访问"));

            if (partialAccess)
            {
                input.Filter.ManagerDepartmentId = user.Data.DepartmentIds[0];
            }

            if (limitedAccess)
            {
                input.Filter.ManagerUserId = _user.Id;
            }

            return await _recordService.PageAsync(input);
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _recordService.DeleteAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(JObject obj)
        {
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordAddInput>(obj["record"].ToString());
            var recordFileTypeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeOutput>>(obj["fileList"].ToString());
            return await _recordService.AddAsync(input, recordFileTypeList);
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Update(JObject obj)
        {
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordUpdateInput>(obj["record"].ToString());
            var recordFileTypeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeUpdateOutput>>(obj["fileList"].ToString());
            return await _recordService.UpdateAsync(input, recordFileTypeList);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetBasicInfo(long id)
        {
            return await _recordService.GetRecordInfoAsync(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetAdditionalInfo(long id)
        {
            return await _recordService.GetRecordAddtionalInfoAsync(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> EditPageReturn(long id)
        {
            var permissionName = await _userService.GetPermissionsNameAsync();

            var fullAccess = permissionName.Any(m => m != null && m.Equals("档案完全访问"));
            var partialAccess = permissionName.Any(m => m != null && m.Equals("档案所属部门访问"));
            var limitedAccess = permissionName.Any(m => m != null && m.Equals("档案所属经理访问"));

            if (fullAccess)
            {
                return await _recordService.GetAsync(id);

            }
            else
            {
                var record = await _recordService.GetRecordAsync(id);

                if (record.Status == 0)
                {
                    return await _recordService.GetAsync(id);
                }
                else
                {
                    return await _recordService.GetRecordAddtionalInfoAsync(id);
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddAdditionalRecordInfo(RecordAdditionalInfoOutput input)
        {
            return await _recordService.AddAdditionalRecordInfoAsync(input.Record, input.RecordFileTypeList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> HandOverPage(PageInput<RecordEntity> input)
        {
            return await _recordService.HandOverPageAsync(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> HandOverCheck(HandOverBasicInfoOutput input)
        {
            return await _recordService.HandOverCheckAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> HandOverBasicInfo(long id)
        {
            return await _recordService.GetHandOverInfoAsync(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetListByUser(long id)
        {
            return await _recordService.GetListByUserAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> RelationChange(List<RecordTransferInput> input)
        {
            return await _recordService.RelationChangeAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPrintInfo(long id)
        {
            return await _recordService.GetPrintInfoAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetExpiredRecordList(PageInput<RecordEntity> input)
        {
            var user = await _userService.GetAsync(_user.Id);
            var permissionName = await _userService.GetPermissionsNameAsync();

            var fullAccess = permissionName.Any(m => m != null && m.Equals("过期档案文件完全访问"));
            var partialAccess = permissionName.Any(m => m != null && m.Equals("过期档案文件所属部门访问"));
            var limitedAccess = permissionName.Any(m => m != null && m.Equals("过期档案文件所属经理访问"));

            var record = new RecordEntity();

            if (partialAccess)
            {
                record.ManagerDepartmentId = user.Data.DepartmentIds[0];
            }

            if (limitedAccess)
            {
                record.ManagerUserId = _user.Id;
            }

            input.Filter = record;

            return await _recordService.GetExpiredRecordListAsync(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> ApplyChangeFile(JObject obj)
        {
            var type = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(obj["type"].ToString());
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeOutput>>(obj["fileList"].ToString());

            return await _recordService.AppleChangeFileAsync(type, input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetChangeDetail(long id)
        {
            return await _recordService.GetChangeDetailAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetApplyChangeList(PageInput<InitiativeUpdateEntity> input)
        {
            return await _recordService.GetApplyChangeListAsync(input);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetApplyChangeDetail(long id)
        {
            return await _recordService.GetApplyChangeDetailAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> AcceptApplyChange(long id)
        {
            return await _recordService.AcceptApplyChangeAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> RefuseApplyChange(JObject obj)
        {
            var id = Newtonsoft.Json.JsonConvert.DeserializeObject<long>(obj["id"].ToString());

            return await _recordService.RefuseApplyChangeAsync(id, obj["refuseReason"].ToString());
        }
    }
}
