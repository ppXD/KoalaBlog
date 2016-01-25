using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using KoalaBlog.Framework.Exceptions;

namespace KoalaBlog.Framework.Util
{
    [DebuggerStepThrough]
    public static class AssertUtil
    {
        /// <summary>
        /// 断言是否正确的Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="zeroBase">Id是否基于零</param>
        /// <param name="message">错误消息</param>
        public static void IsValidId(int id, bool zeroBase = false, string message = "标识必须是非负整数")
        {
            Validator.Instance.IsValidId(id, zeroBase, message).Done();
        }
        /// <summary>
        /// 断言是否正确的Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="zeroBase">Id是否基于零</param>
        /// <param name="message">错误消息</param>
        public static void IsValidId(long id, bool zeroBase = false, string message = "标识必须是非负整数")
        {
            Validator.Instance.IsValidId(id, zeroBase, message).Done();
        }
        /// <summary>
        /// 断言是否正确邮箱地址
        /// </summary>
        /// <param name="email"></param>
        /// <param name="message"></param>
        public static void IsValidEmail(string email, string message)
        {
            Validator.Instance.IsValidEmail(email, message).Done();
        }
        /// <summary>
        /// 断言对象不为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsNotNull<T>(T o, string message = "传入参数不能为空！")
        {
            Validator.Instance.IsNotNull(o, message).Done();
        }

        /// <summary>
        /// 断言对象为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>     
        public static void IsNull<T>(T o, string message = "传入参数必须为空！")
        {
            Validator.Instance.IsNull(o, message).Done();
        }

        /// <summary>
        /// 断言对象不为零
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">出错信息</param>
        public static void IsNotZero<T>(T o, string message = "传入参数必须不等于零！")
        {
            Validator.Instance.IsNotZero(o, message).Done();
        }

        /// <summary>
        /// 断言对象为零
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">出错信息</param>
        public static void IsZero<T>(T o, string message = "传入参数必须等于零！")
        {
            Validator.Instance.IsZero(o, message).Done();
        }

        /// <summary>
        /// 断定对象为指定类型
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">错误提示</param>
        public static void IsInstanceOfType<T>(object o, string message = "传入参数类型错误")
        {
            Validator.Instance.IsInstanceOfType<T>(o, message).Done();
        }

        /// <summary>
        /// 断定对象不为指定类型
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">错误提示</param>
        public static void IsNotInstanceOfType<T>(object o, string message)
        {
            Validator.Instance.IsNotInstanceOfType<T>(o, message).Done();
        }

        /// <summary>
        /// 断定两个对象为相同类型
        /// </summary>        
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">错误提示</param>
        public static void AreSame(object o1, object o2, string message)
        {
            Validator.Instance.AreSame(o1, o2, message).Done();
        }

        /// <summary>
        /// 断定两个对象为不同类型
        /// </summary>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">错误提示</param>
        public static void AreNotSame(object o1, object o2, string message)
        {
            Validator.Instance.AreNotSame(o1, o2, message).Done();
        }


        /// <summary>
        /// 断言对象为真
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsTrue(bool o, string message)
        {
            Validator.Instance.IsTrue(o, message).Done();
        }

        /// <summary>
        /// 断言对象为假
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsFalse(bool o, string message)
        {
            Validator.Instance.IsFalse(o, message).Done();
        }

        /// <summary>
        /// 断言集合对象不为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">集合对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsNotEmptyCollection<T>(ICollection<T> o, string message)
        {
            Validator.Instance.IsNotEmptyCollection<T>(o, message).Done();
        }

        /// <summary>
        /// 断言集合对象为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">集合对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsEmptyCollection<T>(ICollection<T> o, string message)
        {
            Validator.Instance.IsEmptyCollection<T>(o, message).Done();
        }

        /// <summary>
        /// 断言对象等于默认值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsDefault<T>(T o, string message) where T : struct
        {
            Validator.Instance.IsDefault<T>(o, message).Done();
        }

        /// <summary>
        /// 断言对象不等于默认值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsNotDefault<T>(T o, string message) where T : struct
        {
            Validator.Instance.IsNotDefault<T>(o, message).Done();
        }

        #region 比较断言
        /// <summary>
        /// 断言两个对象相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            Validator.Instance.AreEqual<T>(o1, o2, message).Done();
        }

        /// <summary>
        /// 断言两个对象不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreNotEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            Validator.Instance.AreNotEqual<T>(o1, o2, message).Done();
        }

        /// <summary>
        /// 断言对象1到大于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreBigger<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            Validator.Instance.AreBigger<T>(o1, o2, message).Done();
        }

        /// <summary>
        /// 断言对象1到大于等于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreBiggerOrEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            Validator.Instance.AreBiggerOrEqual<T>(o1, o2, message).Done();
        }

        /// <summary>
        /// 断言对象1到小于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreSmaller<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            Validator.Instance.AreSmaller<T>(o1, o2, message).Done();
        }

        /// <summary>
        /// 断言对象1到小于等于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public static void AreSmallerOrEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            Validator.Instance.AreSmallerOrEqual<T>(o1, o2, message).Done();
        }
        #endregion

        #region 范围比较断言
        /// <summary>
        /// 断言对象在值范围内
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public static void Between<T>(T o, T min, T max, string message) where T : IComparable<T>
        {
            Validator.Instance.Between<T>(o, min, max, message).Done();
        }

        /// <summary>
        /// 断言对象不在值范围内
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public static void NotBetween<T>(T o, T min, T max, string message) where T : IComparable<T>
        {
            Validator.Instance.NotBetween<T>(o, min, max, message).Done();
        }

        /// <summary>
        /// 断言存在覆盖
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public static void Coverage<T>(T o1, T o2, T min, T max, string message) where T : IComparable<T>
        {
            Validator.Instance.Coverage<T>(o1, o2, min, max, message).Done();
        }

        /// <summary>
        /// 断言不存在覆盖
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public static void NotCoverage<T>(T o1, T o2, T min, T max, string message) where T : IComparable<T>
        {
            Validator.Instance.NotCoverage<T>(o1, o2, min, max, message).Done();
        }
        #endregion

        /// <summary>
        /// 断言字符串非空并且非空字符组成
        /// </summary>
        /// <param name="o">字符串</param>
        /// <param name="message">异常提示信息</param>
        public static void NotNullOrWhiteSpace(string o, string message)
        {
            Validator.Instance.NotNullOrWhiteSpace(o, message).Done();
        }

        /// <summary>
        /// 断言字符串为空或者由空字符组成
        /// </summary>
        /// <param name="o">字符串</param>
        /// <param name="message">异常提示信息</param>
        public static void NullOrWhiteSpace(string o, string message)
        {
            Validator.Instance.NullOrWhiteSpace(o, message).Done();
        }

        /// <summary>
        /// 判断对象是否存在于枚举类型定义中
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public static void IsInEnum<E>(object o, string message) where E : struct
        {
            Validator.Instance.IsInEnum<E>(o, message).Done();
        }

        /// <summary>
        /// 判断字符串内是否由数字和小数点组成
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="message">异常提示信息</param>
        public static void IsDecimal(string str, string message)
        {
            Validator.Instance.IsDecimal(str, message).Done();
        }

        /// <summary>
        /// 判断字符串内是否由数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="message">异常提示信息</param>
        public static void IsLong(string str, string message)
        {
            Validator.Instance.IsLong(str, message).Done();
        }

        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="message">异常提示信息</param>
        /// <param name="options">正则表达式配置</param>
        public static void IsValid(string input, string pattern, string message, RegexOptions options = RegexOptions.ECMAScript)
        {
            Validator.Instance.IsValid(input, pattern, message, options).Done();
        }

        /// <summary>
        /// 是否为唯一的集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="message">消息</param>
        public static void IsUniqueCollection<T>(IEnumerable<T> list, string message)
        {
            Validator.Instance.IsUniqueCollection<T>(list, message).Done();
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="msg">异常消息</param>
        public static void Fault(string msg)
        {
            Validator.Instance.Fault(msg).Done();
        }

        public static Validator Waterfall()
        {
            return Validator.Instance;
        }
    }

    public sealed class Validator
    {
        private IList<string> exceptionMessageList = new List<string>();

        private Validator() { }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static Validator Instance { get { return new Validator(); } }

        #region 抛出断言异常
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="msg">异常消息</param>
        public Validator Fault(string msg)
        {
            exceptionMessageList.Add(msg);

            return this;
        }

        /// <summary>
        /// 检验完成
        /// </summary>
        public void Done()
        {
            if (exceptionMessageList.Count > 0)
            {
                throw new AssertException(string.Join("~", exceptionMessageList.ToArray()), exceptionMessageList);
            }
        }
        #endregion
        /// <summary>
        /// 断言是否正确的Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="zeroBase">Id是否基于零</param>
        /// <param name="message">错误消息</param>
        public Validator IsValidId(int id, bool zeroBase = false, string message = "标识必须是非负整数")
        {
            if (zeroBase)
                AreBiggerOrEqual(id, 0, message);
            else
                AreBiggerOrEqual(id, 1, message);

            return this;
        }
        /// <summary>
        /// 断言是否正确的Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="zeroBase">Id是否基于零</param>
        /// <param name="message">错误消息</param>
        public Validator IsValidId(long id, bool zeroBase = false, string message = "标识必须是非负整数")
        {
            if (zeroBase)
                AreBiggerOrEqual(id, 0, message);
            else
                AreBiggerOrEqual(id, 1, message);

            return this;
        }
        /// <summary>
        /// 断言是否正确邮箱地址
        /// </summary>
        /// <param name="email"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Validator IsValidEmail(string email, string message)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                Fault(message);
            }
            return this;
        }
        /// <summary>
        /// 断言对象不为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsNotNull<T>(T o, string message = "传入参数不能为空！")
        {
            if (o == null)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>     
        public Validator IsNull<T>(T o, string message = "传入参数必须为空！")
        {
            if (o != null)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象不为零
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">出错信息</param>
        public Validator IsNotZero<T>(T o, string message = "传入参数必须不等于零！")
        {
            if (o.Equals(0))
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象为零
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">出错信息</param>
        public Validator IsZero<T>(T o, string message = "传入参数必须等于零！")
        {
            if (!o.Equals(0))
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断定对象为指定类型
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">错误提示</param>
        public Validator IsInstanceOfType<T>(object o, string message = "传入参数类型错误")
        {
            IsNotNull(o, "待判断对象不能为空！");
            if (!(o is T))
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断定对象不为指定类型
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">错误提示</param>
        public Validator IsNotInstanceOfType<T>(object o, string message)
        {
            IsNotNull(o, "待判断对象不能为空！");
            if (o is T)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断定两个对象为相同类型
        /// </summary>        
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">错误提示</param>
        public Validator AreSame(object o1, object o2, string message)
        {
            IsNotNull(o1, "待判断对象不能为空！");
            IsNotNull(o2, "待判断对象不能为空！");
            if (o1.GetType() != o2.GetType())
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断定两个对象为不同类型
        /// </summary>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">错误提示</param>
        public Validator AreNotSame(object o1, object o2, string message)
        {
            IsNotNull(o1, "待判断对象不能为空！");
            IsNotNull(o2, "待判断对象不能为空！");
            if (o1.GetType() == o2.GetType())
                Fault(message);

            return this;
        }


        /// <summary>
        /// 断言对象为真
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsTrue(bool o, string message)
        {
            if (o)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象为假
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsFalse(bool o, string message)
        {
            if (!o)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言集合对象不为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">集合对象</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsNotEmptyCollection<T>(ICollection<T> o, string message)
        {
            IsNotNull<ICollection<T>>(o, message);
            if (o.Count == 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言集合对象为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">集合对象</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsEmptyCollection<T>(ICollection<T> o, string message)
        {
            IsNotNull<ICollection<T>>(o, message);
            if (o.Count != 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象等于默认值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsDefault<T>(T o, string message) where T : struct
        {
            if (!o.Equals(default(T)))
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象不等于默认值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsNotDefault<T>(T o, string message) where T : struct
        {
            if (o.Equals(default(T)))
                Fault(message);

            return this;
        }

        #region 比较断言
        /// <summary>
        /// 断言两个对象相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public Validator AreEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) != 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言两个对象不相等
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public Validator AreNotEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) == 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象1到大于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public Validator AreBigger<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) <= 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象1到大于等于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public Validator AreBiggerOrEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) < 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象1到小于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public Validator AreSmaller<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) >= 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象1到小于等于对象2
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="message">异常提示信息</param>
        public Validator AreSmallerOrEqual<T>(T o1, T o2, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            if (o1.CompareTo(o2) > 0)
                Fault(message);

            return this;
        }
        #endregion

        #region 范围比较断言
        /// <summary>
        /// 断言对象在值范围内
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public Validator Between<T>(T o, T min, T max, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o, message);
            IsNotNull<T>(min, message);
            IsNotNull<T>(max, message);
            if (!(o.CompareTo(min) >= 0 && o.CompareTo(max) <= 0))
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言对象不在值范围内
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public Validator NotBetween<T>(T o, T min, T max, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o, message);
            IsNotNull<T>(min, message);
            IsNotNull<T>(max, message);
            if (o.CompareTo(min) >= 0 || o.CompareTo(max) <= 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言存在覆盖
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public Validator Coverage<T>(T o1, T o2, T min, T max, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            IsNotNull<T>(min, message);
            IsNotNull<T>(max, message);
            AreSmaller<T>(o1, o2, message);
            if (o1.CompareTo(max) >= 0 || o2.CompareTo(min) <= 0)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言不存在覆盖
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="o1">对象1</param>
        /// <param name="o2">对象2</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">异常提示信息</param>
        public Validator NotCoverage<T>(T o1, T o2, T min, T max, string message) where T : IComparable<T>
        {
            IsNotNull<T>(o1, message);
            IsNotNull<T>(o2, message);
            IsNotNull<T>(min, message);
            IsNotNull<T>(max, message);
            AreSmaller<T>(o1, o2, message);
            if (!(o1.CompareTo(max) > 0 || o2.CompareTo(min) < 0))
                Fault(message);

            return this;
        }
        #endregion

        /// <summary>
        /// 断言字符串非空并且非空字符组成
        /// </summary>
        /// <param name="o">字符串</param>
        /// <param name="message">异常提示信息</param>
        public Validator NotNullOrWhiteSpace(string o, string message)
        {
            if (string.IsNullOrEmpty(o) || o == string.Empty)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 断言字符串为空或者由空字符组成
        /// </summary>
        /// <param name="o">字符串</param>
        /// <param name="message">异常提示信息</param>
        public Validator NullOrWhiteSpace(string o, string message)
        {
            if (!string.IsNullOrEmpty(o) && o != string.Empty)
                Fault(message);

            return this;
        }

        /// <summary>
        /// 判断对象是否存在于枚举类型定义中
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <param name="o">对象</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsInEnum<E>(object o, string message) where E : struct
        {
            if (!System.Enum.IsDefined(typeof(E), o))
                Fault(message);

            return this;
        }

        /// <summary>
        /// 判断字符串内是否由数字和小数点组成
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsDecimal(string str, string message)
        {
            NotNullOrWhiteSpace(str, message);
            try
            {
                decimal.Parse(str);
            }
            catch
            {
                Fault(message);
            }

            return this;
        }

        /// <summary>
        /// 判断字符串内是否由数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="message">异常提示信息</param>
        public Validator IsLong(string str, string message)
        {
            NotNullOrWhiteSpace(str, message);
            try
            {
                long.Parse(str);
            }
            catch
            {
                Fault(message);
            }

            return this;
        }

        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="message">异常提示信息</param>
        /// <param name="options">正则表达式配置</param>
        public Validator IsValid(string input, string pattern, string message, RegexOptions options = RegexOptions.ECMAScript)
        {
            Regex regex = new Regex(pattern, options);
            IsFalse(regex.IsMatch(input), message);

            return this;
        }

        /// <summary>
        /// 是否为唯一的集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="message">消息</param>
        public Validator IsUniqueCollection<T>(IEnumerable<T> list, string message)
        {
            AreEqual(list.Distinct().Count(), list.Count(), message);

            return this;
        }
    }
}
