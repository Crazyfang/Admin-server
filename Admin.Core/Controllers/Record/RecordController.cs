using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Core.Attributes;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Hubs;
using Admin.Core.Model.Record;
using Admin.Core.Service.Admin.Department;
using Admin.Core.Service.Admin.User;
using Admin.Core.Service.Record.Notify;
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
    /// <summary>
    /// 信贷档案
    /// </summary>
    public class RecordController:AreaController
    {
        private readonly IUserService _userService;
        private readonly IRecordService _recordService;
        private readonly IDepartmentService _departmentService;
        private readonly IUser _user;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly INotifyService _notifyService;

        public RecordController(IUser user
            , IRecordService recordService
            , IUserService userService
            , IHubContext<ChatHub> hubContext
            , IDepartmentService departmentService
            , INotifyService notifyService)
        {
            _user = user;
            _recordService = recordService;
            _userService = userService;
            _hubContext = hubContext;
            _departmentService = departmentService;
            _notifyService = notifyService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Test()
        {
            await _hubContext.Clients.Group("Admin").SendAsync("Show", "测试文字");
            return ResponseOutput.Ok();
        }

        /// <summary>
        /// 根据档案主键获取档案信息
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _recordService.GetAsync(id);
        }

        /// <summary>
        /// 档案列表分页
        /// </summary>
        /// <param name="input">分页参数及筛选条件</param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据档案主键删除档案
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        public async Task<IResponseOutput> Delete(long id)
        {
            return await _recordService.DeleteAsync(id);
        }

        /// <summary>
        /// 添加档案信息
        /// </summary>
        /// <param name="obj">档案信息(包括档案基本信息和文件信息)</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> Add(JObject obj)
        {
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordAddInput>(obj["record"].ToString());
            var recordFileTypeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeOutput>>(obj["fileList"].ToString());
            return await _recordService.AddAsync(input, recordFileTypeList);
        }

        /// <summary>
        /// 更新档案信息
        /// </summary>
        /// <param name="obj">档案信息(包括档案基本信息和文件信息)</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IResponseOutput> Update(JObject obj)
        {
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordUpdateInput>(obj["record"].ToString());
            var recordFileTypeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeUpdateOutput>>(obj["fileList"].ToString());
            return await _recordService.UpdateAsync(input, recordFileTypeList);
        }

        /// <summary>
        /// 获取档案基础信息
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetBasicInfo(long id)
        {
            return await _recordService.GetRecordInfoAsync(id);
        }

        /// <summary>
        /// 获取档案补充提交基础信息(只包含文件类别，不包含文件)
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetAdditionalInfo(long id)
        {
            return await _recordService.GetRecordAddtionalInfoAsync(id);
        }

        /// <summary>
        /// 根据权限判断是返回编辑界面还是补充提交界面
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
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

        /// <summary>
        /// 提交档案补充提交信息
        /// </summary>
        /// <param name="input">补充提交文件信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> AddAdditionalRecordInfo(RecordAdditionalInfoOutput input)
        {
            return await _recordService.AddAdditionalRecordInfoAsync(input.Record, input.RecordFileTypeList);
        }

        /// <summary>
        /// 移交档案分页
        /// </summary>
        /// <param name="input">分页信息及筛选条件</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> HandOverPage(PageInput<RecordEntity> input)
        {
            return await _recordService.HandOverPageAsync(input);
        }

        /// <summary>
        /// 移交确认
        /// </summary>
        /// <param name="input">需要移交的档案及其归属文件信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> HandOverCheck(HandOverBasicInfoOutput input)
        {
            //var data = await _recordService.HandOverCheckAsync(input);
            //var record = await _recordService.GetRecordAsync(input.Record.Id);

            //_notifyService.Insert(input.Record.Id, $"{input.Record.RecordId}档案移交成功");

            //if (RedisHelper.HExists("signalR", input.Record.ManagerUserId.Value.ToString()))
            //{
            //    await _hubContext.Clients.Client(RedisHelper.HGet("signalR", input.Record.ManagerUserId.Value.ToString())).SendAsync("Show", "信息刷新", $"您有一份档案移交成功");
            //}
            //return data;
            var data =  _recordService.HandOverCheckAsync(input);
            await _notifyService.InsertAsync(input.Record.ManagerUserId.Value, $"{input.Record.RecordId}移交成功!");
            if (RedisHelper.HExists("signalR", input.Record.ManagerUserId.Value.ToString()))
            {
                await _hubContext.Clients.Client(RedisHelper.HGet("signalR", input.Record.ManagerUserId.Value.ToString())).SendAsync("Show", "信息刷新", $"您有一份档案移交成功");
            }
            return data;
        }

        /// <summary>
        /// 移交基本信息
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> HandOverBasicInfo(long id)
        {
            return await _recordService.GetHandOverInfoAsync(id);
        }

        /// <summary>
        /// 根据用户返回归管的档案ID及一些信息
        /// </summary>
        /// <param name="id">用户主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetListByUser(long id)
        {
            return await _recordService.GetListByUserAsync(id);
        }

        /// <summary>
        /// 档案归属关系变更提交
        /// </summary>
        /// <param name="input">档案归属关系变更信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> RelationChange(List<RecordTransferInput> input)
        {
            return await _recordService.RelationChangeAsync(input);
        }

        /// <summary>
        /// 档案转让权限返回
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> RelationPagePermissions()
        {
            var permissionName = await _userService.GetPermissionsNameAsync();

            var fullAccess = permissionName.Any(m => m != null && m.Equals("档案转让完全访问"));
            var partialAccess = permissionName.Any(m => m != null && m.Equals("档案转让所属部门访问"));
            var limitedAccess = permissionName.Any(m => m != null && m.Equals("档案转让所属经理访问"));

            if (fullAccess)
            {
                return ResponseOutput.Ok(new { DepartmentId = 0, UserId = 0 });
            }

            var user = await _userService.GetAsync(_user.Id);
            if (partialAccess)
            {
                return ResponseOutput.Ok(new { DepartmentId = user.Data.DepartmentIds.First(), UserId = 0 });
            }

            return ResponseOutput.Ok(new { DepartmentId = user.Data.DepartmentIds.First(), UserId = _user.Id });

        }

        /// <summary>
        /// 获取档案打印基本信息
        /// </summary>
        /// <param name="id">档案主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetPrintInfo(long id)
        {
            return await _recordService.GetPrintInfoAsync(id);
        }

        /// <summary>
        /// 过期档案分页查询(有权限限制)
        /// </summary>
        /// <param name="input">分页信息及筛选条件</param>
        /// <returns></returns>
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

        /// <summary>
        /// 申请更改档案文件
        /// </summary>
        /// <param name="obj">type-类型(主动更新还是过期更新),fileList-文件列表</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> ApplyChangeFile(JObject obj)
        {
            var type = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(obj["type"].ToString());
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeOutput>>(obj["fileList"].ToString());

            return await _recordService.AppleChangeFileAsync(type, input);
        }

        /// <summary>
        /// 获取更换的档案文件具体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetChangeDetail(long id)
        {
            return await _recordService.GetChangeDetailAsync(id);
        }

        /// <summary>
        /// 申请更换档案文件分页
        /// </summary>
        /// <param name="input">分页信息及筛选条件</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetApplyChangeList(PageInput<InitiativeUpdateEntity> input)
        {
            return await _recordService.GetApplyChangeListAsync(input);
        }

        /// <summary>
        /// 获取申请更换的档案文件具体信息
        /// </summary>
        /// <param name="id">申请更换主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetApplyChangeDetail(long id)
        {
            return await _recordService.GetApplyChangeDetailAsync(id);
        }

        /// <summary>
        /// 接受申请更换档案文件
        /// </summary>
        /// <param name="id">申请更换主键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IResponseOutput> AcceptApplyChange(long id)
        {
            var record = await _recordService.GetRecordByIniId(id);
            var data = await _recordService.AcceptApplyChangeAsync(id);

            if (RedisHelper.HExists("signalR", record.ManagerUserId.Value.ToString()))
            {
                await _hubContext.Clients.Client(RedisHelper.HGet("signalR", record.ManagerUserId.Value.ToString())).SendAsync("Show", "信息刷新", $"您申请更新文件成功");
            }
            return data;
        }

        /// <summary>
        /// 拒绝申请更换档案文件
        /// </summary>
        /// <param name="obj">id-申请更换主键,refuseReason-拒绝原因</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> RefuseApplyChange(JObject obj)
        {
            var id = Newtonsoft.Json.JsonConvert.DeserializeObject<long>(obj["id"].ToString());

            var data = await _recordService.RefuseApplyChangeAsync(id, obj["refuseReason"].ToString());
            var record = await _recordService.GetRecordByIniId(id);

            await _notifyService.InsertAsync(record.ManagerUserId.Value, $"{record.RecordId}申请更新文件被拒绝,原因为: " + obj["refuseReason"].ToString());

            if (RedisHelper.HExists("signalR", record.ManagerUserId.Value.ToString()))
            {
                await _hubContext.Clients.Client(RedisHelper.HGet("signalR", record.ManagerUserId.Value.ToString())).SendAsync("Show", "信息刷新", $"您申请更新文件被拒绝，原因为:{obj["refuseReason"]}");
            }
            return data;
        }

        /// <summary>
        /// 获取待创建档案分页
        /// </summary>
        /// <param name="input">分页信息及筛选条件</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> GetNeedCreateRecordList(PageInput<NeedCreateRecordEntity> input)
        {
            var permissionName = await _userService.GetPermissionsNameAsync();

            var fullAccess = permissionName.Any(m => m != null && m.Equals("待创建档案完全访问"));
            var partialAccess = permissionName.Any(m => m != null && m.Equals("待创建档案部门访问"));
            var limitedAccess = permissionName.Any(m => m != null && m.Equals("待创建档案经理访问"));

            if (fullAccess)
            {
                return await _recordService.GetNeedCreateRecordList(0, "8280000", 0, input);
            }

            var user = await _userService.GetAsync(_user.Id);
            if (partialAccess)
            {
                return await _recordService.GetNeedCreateRecordList(1, "8280000", user.Data.DepartmentIds.FirstOrDefault(), input);
            }

            return await _recordService.GetNeedCreateRecordList(2, user.Data.UserName, user.Data.DepartmentIds.FirstOrDefault(), input);
        }

        /// <summary>
        /// 拒绝移交
        /// </summary>
        /// <param name="obj">id-档案主键,reason-拒绝原因</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> RefuseHandOver(JObject obj)
        {
            var id = Newtonsoft.Json.JsonConvert.DeserializeObject<long>(obj["id"].ToString());
            var reason = obj["reason"].ToString();
            var record = await _recordService.GetRecordAsync(id);

            await _notifyService.InsertAsync(record.ManagerUserId.Value, $"{record.RecordId}被退回,原因为: " + reason);

            if (RedisHelper.HExists("signalR", record.ManagerUserId.Value.ToString()))
            {
                await _hubContext.Clients.Client(RedisHelper.HGet("signalR", record.ManagerUserId.Value.ToString())).SendAsync("Show", "信息刷新", $"您有一份档案被退回，原因为:{reason}");
            }

            return ResponseOutput.Ok();
        }

        /// <summary>
        /// 存量档案添加
        /// </summary>
        /// <param name="obj">档案信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResponseOutput> StockAdd(JObject obj)
        {
            var input = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordAddInput>(obj["record"].ToString());
            var recordFileTypeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecordFileTypeOutput>>(obj["fileList"].ToString());
            return await _recordService.StockAddAsync(input, recordFileTypeList);
        }
    }
}
