using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Entity.Models.Enums
{
    /// <summary>
    /// AccessControl的AccessLevel枚举，主要用于控制发表Blog的访问控制
    /// </summary>
    public enum BlogInfoAccessInfo
    {
        [Description("仅自己可见")]
        MyselfOnly = 0,
        [Description("好友圈")]
        FriendOnly = 1,
        [Description("公开")]
        All = 2,
        [Description("群可见")]
        GroupOnly = 3,
    }
}
