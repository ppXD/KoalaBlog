using KoalaBlog.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.ApiClient.Extensions
{
    public static class PersonClientExtension
    {
        public static PersonClient CreatePersonClient(this ApiClients source)
        {
            return (CreatePersonClient(source, AppSettingConfig.BaseAddress));
        }

        public static PersonClient CreatePersonClient(this ApiClients source, string serviceUrl)
        {
            return new PersonClient(new Uri(serviceUrl));
        }
    }
}
