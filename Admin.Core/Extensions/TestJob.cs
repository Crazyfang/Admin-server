using System;
using System.Threading.Tasks;
using Admin.Core.Service.Admin.User;
using Quartz;

namespace Admin.Core.Extensions
{
    [DisallowConcurrentExecution]
    public class TestJob:IJob
    {
        public IUserService _userService;
        public TestJob(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var result = await _userService.GetAsync(2);
            await Console.Out.WriteLineAsync($"{DateTime.Now.ToShortTimeString()}-{result.Data.NickName}");
        }
    }
}
