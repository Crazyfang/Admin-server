using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.Core.Service.Questionnaire.UserPower
{
    public interface IUserPowerService
    {
        /// <summary>
        /// 获取可以访问的公议列表归属地
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// <returns></returns>
        Task<List<string>> GetPowerListAsync(long userId);
    }
}
