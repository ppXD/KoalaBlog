using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Util
{
    [Serializable]
    public class AssertMessage
    {
        public IList<string> AssertMessageList { get; set; }
    }
}
