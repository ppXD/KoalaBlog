using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.MVC
{
    public class ActionResultData
    {
        public ActionResultData() { }

        public ActionResultData(bool isSucceed, string title)
        {

        }

        public bool IsSucceed { get; set; }

        public bool IsException { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

    }

    public class ActionResultData<T> : ActionResultData
    {
        public T Data { get; set; }
    }
}
