using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.Core.Repository.Questionnaire.UserPower;

namespace Admin.Core.Service.Questionnaire.UserPower
{
    public class UserPowerService:IUserPowerService
    {
        private readonly IUserPowerRepository _userPowerRepostitory;
        public UserPowerService(IUserPowerRepository userPowerRepository)
        {
            _userPowerRepostitory = userPowerRepository;
        }

        public async Task<List<string>> GetPowerListAsync(long userId)
        {
            var entity = await _userPowerRepostitory.Select
                .Where(i => i.UserId == userId)
                .ToOneAsync();

            if(entity == null)
            {
                return new List<string>();
            }
            else
            {
                var list = entity.Power.Split(',');

                return new List<string>(list);
            }
            
        }
    }
}
