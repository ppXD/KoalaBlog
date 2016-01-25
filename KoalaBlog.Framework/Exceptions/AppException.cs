using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Exceptions
{
    /// <summary>
    /// 应用异常
    /// </summary>
    [Serializable]
    public class AppException : ApplicationException
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public AppException()
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="message">消息</param>
        public AppException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public AppException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        public AppException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
