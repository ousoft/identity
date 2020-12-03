using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Utility
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取UTC时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>单位（秒）</returns>
        public static long ToTimestamp(DateTime dateTime) => (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1).ToUniversalTime()).TotalSeconds;

        /// <summary>
        /// 获取本地时间
        /// </summary>
        /// <param name="timestamp">单位（秒）</param>
        /// <returns></returns>
        public static DateTime ToDateTime(long timestamp) => new DateTime(1970, 1, 1).ToUniversalTime().AddSeconds(timestamp + DateTimeOffset.Now.Offset.TotalSeconds);
    }
}
