using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace KoalaBlog.WebApiHost
{
    public class WebApiLoader : DefaultAssembliesResolver
    {
        public override ICollection<System.Reflection.Assembly> GetAssemblies()
        {
            var assemblyList = new List<System.Reflection.Assembly>
            {
                AppDomain.CurrentDomain.Load("KoalaBlog.WebApi")
            };
            return assemblyList;
        }
    }
}
