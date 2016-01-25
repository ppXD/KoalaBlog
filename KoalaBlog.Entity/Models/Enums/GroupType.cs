using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Entity.Models.Enums
{
    public enum GroupType
    {
        [Description("群名单")]
        GroupList = 0,
        [Description("黑名单")]
        BlackList = 1,
        [Description("屏蔽名单")]
        ShieldList = 2
    }
}