using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Entity.Models.Enums
{
    public enum ContentType
    {
       [Description("图片")]
       Photo = 1,
       [Description("视频")]
       Video = 2,
       [Description("音乐")]
       Music = 3
    }
}
