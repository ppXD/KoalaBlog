using KoalaBlog.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.ApiClient.Extensions
{
    public static class ContentClientExtension
    {
        public static ContentClient CreateContentClient(this ApiClients source)
        {
            return (CreateContentClient(source, AppSettingConfig.BaseAddress));
        }

        public static ContentClient CreateContentClient(this ApiClients source, string serviceUrl)
        {
            return new ContentClient(new Uri(serviceUrl));
        }
    }
}
