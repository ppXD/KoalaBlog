using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Entity.Models.Enums
{
    /// <summary>
    /// 访问控制
    /// </summary>
    public enum PersonInfoAccessInfo
    {
        [Description("仅自己可见")]
        MyselfOnly = 0,
        [Description("我关注的人可见")]
        FollowerOnly = 1,
        [Description("所有人可见")]
        All = 2
    }

    public enum AllowablePersonForComment
    {
        [Description("我关注的人")]
        FollowerOnly = 0,
        [Description("仅粉丝")]
        FansOnly = 1,
        [Description("所有人")]
        All = 2
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        [Description("男")]
        Male = 0,
        [Description("女")]
        Female = 1
    }

    /// <summary>
    /// 性取向
    /// </summary>
    public enum SexualTrend
    {
        [Description("男")]
        Male = 0,
        [Description("女")]
        Female = 1,
        [Description("男女")]
        MaleAndFemale = 2
    }

    /// <summary>
    /// 感情/婚姻状况
    /// </summary>
    public enum MaritalStatus
    {
        [Description("单身")]
        Single = 0,
        [Description("已婚")]
        Married = 1,
        [Description("未婚")]
        UnMarried = 2
    }

    /// <summary>
    /// 血型
    /// </summary>
    public enum BloodType
    {
        [Description("A型")]
        A = 0,
        [Description("B型")]
        B = 1,
        [Description("O型")]
        O = 2,
        [Description("AB型")]
        AB = 3
    }
}
