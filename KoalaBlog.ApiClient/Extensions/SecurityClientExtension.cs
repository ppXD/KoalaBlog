using KoalaBlog.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.ApiClient.Extensions
{
    public static class SecurityClientExtension
    {
        public static SecurityClient CreateSecurityClient(this ApiClients source)
        {
            return(CreateSecurityClient(source, AppSettingConfig.BaseAddress));
        }

        public static SecurityClient CreateSecurityClient(this ApiClients source, string serviceUrl)
        {
            return new SecurityClient(new Uri(serviceUrl));
        }
    }
}
