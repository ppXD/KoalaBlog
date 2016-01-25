using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Extensions
{
    public static class SerializeExtension
    {
        #region JSON格式
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">原始对象</param>
        /// <returns>JSON格式字符串</returns>
        public static string ToJSON(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// JSON字符串转换为对象
        /// </summary>
        /// <param name="json">JSON格式字符串</param>
        /// <returns>对象</returns>
        public static T FromJSON<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        #endregion
    }
}
