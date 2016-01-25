using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Exceptions
{
    /// <summary>
    /// 断言异常
    /// </summary>
    [Serializable]
    public class AssertException : AppException
    {
        private IList<string> exceptionMessageList = null;

        /// <summary>
        /// 获取异常信息列表
        /// </summary>
        public IList<string> ExceptionMessageList
        {
            get
            {
                return exceptionMessageList;
            }
            set
            {
                this.exceptionMessageList = value;
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public AssertException()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息</param>
        public AssertException(string message, IList<string> exceptionMessageList)
            : base(message)
        {
            this.exceptionMessageList = exceptionMessageList;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public AssertException(string message, IList<string> exceptionMessageList, Exception innerException)
            : base(message, innerException)
        {
            this.exceptionMessageList = exceptionMessageList;
        }
        public AssertException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
