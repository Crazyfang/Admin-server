using System;
namespace Admin.Core.Common.Helpers
{
    public class RedisDataHelper
    {
        public static long IncrIndex(string key)
        {
            var index = RedisHelper.IncrBy(key);
            return index;
        }

        public static bool SetIndex(string key, long value = 1)
        {
            var index = RedisHelper.SetNx(key, value);
            return index;
        }
    }
}
