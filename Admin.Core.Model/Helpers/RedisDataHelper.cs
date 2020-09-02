using System;
using Admin.Core.Common.Attributes;
using Admin.Core.Enums;
using Admin.Core.Model.Record;

namespace Admin.Core.Common.Helpers
{
    public class RedisDataHelper
    {
        private readonly IFreeSql _freeSql;

        public RedisDataHelper(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }

        public async void Seed()
        {
            foreach(var item in Enum.GetNames(typeof(DepartmentCode)))
            {
                if (Exists(item))
                {
                    continue;
                }
                else
                {
                    var res = await _freeSql.Select<DepartmentSeeddEntity>().Where(i => i.DepartmentName == item).ToOneAsync();
                    SetIndex(item, res.Seed.ToLong());
                    Console.WriteLine($"{item} 种子 {res.Seed} 写入成功!");
                }
            }
        }

        public long IncrIndex(string key)
        {
            if (!Exists(key))
            {
                Seed();
            }

            var index = RedisHelper.IncrBy(key);
            _freeSql.Update<DepartmentSeeddEntity>().Set(i => i.Seed, index + 1).Where(i => i.DepartmentName == key).ExecuteAffrows();
            return index;
        }

        public bool SetIndex(string key, long value = 1)
        {
            var index = RedisHelper.SetNx(key, value);
            return index;
        }

        public bool Exists(string key)
        {
            return RedisHelper.Exists(key);
        }
    }
}
